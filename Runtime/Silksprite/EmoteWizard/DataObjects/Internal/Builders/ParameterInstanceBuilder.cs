using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal.Builders
{
    public class ParameterInstanceBuilder
    {
        string _name = "";
        ParameterItemKind _itemKind;
        bool _saved = true;
        float _defaultValue;
        bool _synced = true;
        readonly HashSet<string> _referenceUsages = new HashSet<string>();
        readonly List<ParameterWriteUsage> _writeUsages = new List<ParameterWriteUsage>();
        readonly List<ParameterReadUsage> _readUsages = new List<ParameterReadUsage>();

        public string Name => _name;
        public bool HasWriteUsages => _writeUsages.Count > 0;

        ParameterInstanceBuilder() { }

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
            AddSynced();
            if (_writeUsages.All(state => !state.Value.IsDefault))
            {
                _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Default, 0, ParameterWriteSourceKind.NoUI));
            }
        }

        void AddReadDefault()
        {
            AddSynced();
            if (_readUsages.All(state => !state.Value.IsDefault))
            {
                _readUsages.Add(new ParameterReadUsage(ParameterItemKind.Auto, 0));
            }
        }

        public void AddWriteValue(ParameterWriteUsageKind kind, float value, ParameterWriteSourceKind sourceKind)
        {
            AddWriteDefault();
            _writeUsages.Add(new ParameterWriteUsage(kind, value, sourceKind));
        }

        public void AddWriteValue(float value, ParameterWriteSourceKind sourceKind)
        {
            AddWriteDefault();
            if (value > 1)
            {
                _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Int, value, sourceKind));
            }
            else if (Mathf.Abs(value % 1f) > 0f)
            {
                _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Float, value, sourceKind));
            }
            else if (value != 0)
            {
                _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Int, value, sourceKind));
            }
        }

        public void AddReadValue(ParameterItemKind itemKind, float value)
        {
            AddReadDefault();
            _readUsages.Add(new ParameterReadUsage(itemKind, value));
        }

        public void AddWritePuppet(ParameterWriteSourceKind sourceKind)
        {
            if (sourceKind is ParameterWriteSourceKind.TwoAxisPuppet)
            {
                _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Float, -1f, sourceKind));
            }
            AddWriteDefault();
            _writeUsages.Add(new ParameterWriteUsage(ParameterWriteUsageKind.Float, 1f, sourceKind));
        }

        public void AddReferenceUsage(string name)
        {
            _referenceUsages.Add(name);
        }

        public void AddSynced()
        {
            _synced = true;
        }

        public void Import(ParameterItem parameter)
        {
            _name = parameter.name;
            _itemKind = parameter.itemKind;
            _saved |= parameter.saved;
            _defaultValue = parameter.defaultValue;
            _synced |= parameter.synced;
            AddReferenceUsage(parameter.name);
        }

        public ParameterInstance ToInstance()
        {
            return new ParameterInstance
            {
                name = _name,
                saved = _saved,
                defaultValue = ParameterValue.Create(_itemKind, _defaultValue),
                synced = _synced,
                itemKind = _itemKind,
                referenceUsages = _referenceUsages.ToList(),
                writeUsages = _writeUsages
                    .DistinctBy(writeUsage => (writeUsage.writeUsageKind, writeUsage.Value))
                    .OrderBy(usage => usage.Value)
                    .ToList(),
                readUsages = _readUsages
                    .DistinctBy(usage => usage.Value)
                    .OrderBy(usage => usage.Value)
                    .ToList()
            };
        }
    }
}