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
        readonly List<ParameterUsage> _usages = new List<ParameterUsage>();

        public string Name => Name;

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

        void AddDefaultUsage()
        {
            if (_usages.All(state => state.value != 0))
            {
                _usages.Add(new ParameterUsage(ParameterUsageKind.Default, 0));
            }
        }

        public void AddUsage(float value)
        {
            AddDefaultUsage();
            if (value > 1)
            {
                _usages.Add(new ParameterUsage(ParameterUsageKind.Int, value));
            }
            else if (Mathf.Abs(value % 1f) > 0f)
            {
                _usages.Add(new ParameterUsage(ParameterUsageKind.Float, value));
            }
            else if (value != 0)
            {
                _usages.Add(new ParameterUsage(ParameterUsageKind.Int, value));
            }
        }

        public void AddPuppetUsage(bool hasNegativeRange)
        {
            if (hasNegativeRange) _usages.Add(new ParameterUsage(ParameterUsageKind.Float, -1f));
            AddDefaultUsage();
            _usages.Add(new ParameterUsage(ParameterUsageKind.Float, 1f));
        }

        public void AddIndexUsage()
        {
            AddDefaultUsage();
            _itemKind = ParameterItemKind.Int;
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
                Usages = _usages.Select(state => (valueKind: state.usageKind, state.value))
                    .Distinct()
                    .Select(usageValue => new ParameterUsage(usageValue.valueKind, usageValue.value))
                    .OrderBy(usage => usage.value)
                    .ToList()
            };
        }
    }
}