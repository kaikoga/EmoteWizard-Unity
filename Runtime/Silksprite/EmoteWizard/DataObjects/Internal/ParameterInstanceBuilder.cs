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
        bool _synced = true;
        readonly List<ParameterWriteUsage> _writeUsages = new List<ParameterWriteUsage>();
        readonly List<ParameterReadUsage> _readUsages = new List<ParameterReadUsage>();

        public string Name => _name;
        public bool HasWriteUsages => _writeUsages.Count > 0;

        public static ParameterInstanceBuilder Populate(string name)
        {
            return new ParameterInstanceBuilder
            {
                _name = name,
                _itemKind = ParameterItemKind.Auto,
                _saved = false,
                _defaultValue = 0,
                _synced = false
            };
        }

        void AddWriteDefault()
        {
            if (_writeUsages.All(state => state.Value != 0))
            {
                _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Default, 0));
            }
        }

        void AddReadDefault()
        {
            if (_readUsages.All(state => state.Value != 0))
            {
                _readUsages.Add(new ParameterReadUsage(ParameterItemKind.Auto, 0));
            }
        }

        public void AddWriteValue(float value)
        {
            AddWriteDefault();
            if (value > 1)
            {
                _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Int, value));
            }
            else if (Mathf.Abs(value % 1f) > 0f)
            {
                _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Float, value));
            }
            else if (value != 0)
            {
                _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Int, value));
            }
        }

        public void AddReadValue(ParameterItemKind itemKind, float value)
        {
            AddReadDefault();
            _readUsages.Add(new ParameterReadUsage(itemKind, value));
        }

        public void AddWritePuppet(bool hasNegativeRange)
        {
            if (hasNegativeRange) _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Float, -1f));
            AddWriteDefault();
            _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Float, 1f));
        }

        public void AddSynced()
        {
            _synced = true;
        }

        public void Import(ParameterItem parameter)
        {
            _name = parameter.name;
            _itemKind = parameter.itemKind;
            _saved = parameter.saved;
            _defaultValue = parameter.defaultValue;
            _synced = parameter.synced;
        }

        public ParameterInstance ToInstance()
        {
            return new ParameterInstance
            {
                Name = _name,
                Saved = _saved,
                DefaultValue = _defaultValue,
                Synced = _synced,
                ItemKind = _itemKind,
                WriteUsages = _writeUsages.Select(state => (valueKind: state.WriteUsageKind, value: state.Value))
                    .Distinct()
                    .Select(usageValue => new ParameterWriteUsage(usageValue.valueKind, usageValue.value))
                    .OrderBy(usage => usage.Value)
                    .ToList(),
                ReadUsages = _readUsages.GroupBy(state => state.Value)
                    .Select(group => group.First())
                    .OrderBy(usage => usage.Value)
                    .ToList()
            };
        }
    }
}