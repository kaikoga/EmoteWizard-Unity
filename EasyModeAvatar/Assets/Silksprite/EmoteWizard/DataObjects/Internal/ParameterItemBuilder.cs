using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Sources;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParameterItemBuilder
    {
        public string name { get; private set; }
        public bool enabled { get; private set; }

        ParameterItemKind itemKind;
        bool saved = true;
        float defaultValue;
        readonly List<ParameterUsage> usages = new List<ParameterUsage>();

        public static ParameterItemBuilder Populate(string name)
        {
            return new ParameterItemBuilder
            {
                enabled = false,
                name = name,
                itemKind = ParameterItemKind.Auto,
                saved = false,
                defaultValue = 0,
            };
        }

        public void Enable()
        {
            enabled = true;
        }

        void AddDefaultUsage()
        {
            if (usages.All(state => state.value != 0))
            {
                usages.Add(new ParameterUsage(ParameterUsageKind.Default, 0));
            }
        }

        public void AddUsage(float value)
        {
            AddDefaultUsage();
            if (value > 1)
            {
                usages.Add(new ParameterUsage(ParameterUsageKind.Int, value));
            }
            else if (Mathf.Abs(value % 1f) > 0f)
            {
                usages.Add(new ParameterUsage(ParameterUsageKind.Float, value));
            }
            else if (value != 0)
            {
                usages.Add(new ParameterUsage(ParameterUsageKind.Int, value));
            }
        }

        public void AddPuppetUsage(bool hasNegativeRange)
        {
            if (hasNegativeRange) usages.Add(new ParameterUsage(ParameterUsageKind.Float, -1f));
            AddDefaultUsage();
            usages.Add(new ParameterUsage(ParameterUsageKind.Float, 1f));
        }

        public void AddIndexUsage()
        {
            AddDefaultUsage();
            itemKind = ParameterItemKind.Int;
        }

        public void Import(ParameterItem parameter)
        {
            enabled = parameter.enabled;
            name = parameter.name;
            itemKind = parameter.itemKind;
            saved = parameter.saved;
            defaultValue = parameter.defaultValue;
            if (parameter.usages != null) usages.AddRange(parameter.usages);
        }

        public ParameterItem Export()
        {
            return new ParameterItem
            {
                enabled = enabled,
                name = name,
                saved = saved,
                defaultValue = defaultValue,
                itemKind = itemKind,
                usages = usages.Select(state => (valueKind: state.usageKind, state.value))
                    .Distinct()
                    .Select(usageValue => new ParameterUsage(usageValue.valueKind, usageValue.value))
                    .OrderBy(usage => usage.value)
                    .ToList()
            };
        }
    }
}