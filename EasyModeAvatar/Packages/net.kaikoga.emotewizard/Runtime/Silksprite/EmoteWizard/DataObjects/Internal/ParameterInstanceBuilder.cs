using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParameterInstanceBuilder
    {
        string _name;
        ParameterItemKind _itemKind;
        bool _saved = true;
        float _defaultValue;
        readonly List<ParameterWriteUsage> _usages = new List<ParameterWriteUsage>();

        public string Name => _name;

        public static ParameterInstanceBuilder Populate(string name)
        {
            return new ParameterInstanceBuilder
            {
                _name = name,
                _itemKind = ParameterItemKind.Auto,
                _saved = false,
                _defaultValue = 0,
            };
        }

        void AddWriteDefault()
        {
            if (_usages.All(state => state.Value != 0))
            {
                _usages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Default, 0));
            }
        }

        public void AddWriteValue(float value)
        {
            AddWriteDefault();
            if (value > 1)
            {
                _usages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Int, value));
            }
            else if (Mathf.Abs(value % 1f) > 0f)
            {
                _usages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Float, value));
            }
            else if (value != 0)
            {
                _usages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Int, value));
            }
        }

        public void AddWritePuppet(bool hasNegativeRange)
        {
            if (hasNegativeRange) _usages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Float, -1f));
            AddWriteDefault();
            _usages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Float, 1f));
        }

        public void Import(ParameterItem parameter)
        {
            _name = parameter.name;
            _itemKind = parameter.itemKind;
            _saved = parameter.saved;
            _defaultValue = parameter.defaultValue;
        }

        public ParameterInstance ToInstance()
        {
            return new ParameterInstance
            {
                Name = _name,
                Saved = _saved,
                DefaultValue = _defaultValue,
                ItemKind = _itemKind,
                WriteUsages = _usages.Select(state => (valueKind: state.WriteUsageKind, value: state.Value))
                    .Distinct()
                    .Select(usageValue => new ParameterWriteUsage(usageValue.valueKind, usageValue.value))
                    .OrderBy(usage => usage.Value)
                    .ToList()
            };
        }
    }
}