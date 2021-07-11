using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParameterItemBuilder
    {
        public string name;
        bool saved = true;
        float defaultValue;
        bool defaultParameter;
        readonly List<ParameterUsage> usages = new List<ParameterUsage>();

        ParameterValueKind GuessValueKind()
        {
            if (usages.Any(usage => usage.usageKind == ParameterUsageKind.Float)) return ParameterValueKind.Float;
            return usages.Count(usage => usage.usageKind != ParameterUsageKind.Default) > 1 ? ParameterValueKind.Int : ParameterValueKind.Bool;
        }

        public static ParameterItemBuilder Populate(string name)
        {
            return new ParameterItemBuilder
            {
                name = name,
                saved = false,
                defaultValue = 0,
                defaultParameter = false
            };
        }

        public void AddUsage(float value)
        {
            if (value > 1)
            {
                usages.Add(new ParameterUsage(ParameterUsageKind.Int, value));
            }
            else if (Mathf.Abs(value % 1f) > 0f)
            {
                usages.Add(new ParameterUsage(ParameterUsageKind.Float, value));
            }
        }

        public void AddPuppetUsage()
        {
            if (usages.All(state => state.value != 0))
            {
                usages.Add(new ParameterUsage(ParameterUsageKind.Default, 0));
            }

            usages.Add(new ParameterUsage(ParameterUsageKind.Float, 1f));
        }

        public void Import(VRCExpressionParameters.Parameter parameter)
        {
            name = parameter.name;
            saved = parameter.saved;
            defaultValue = parameter.defaultValue;
            AddUsage(parameter.defaultValue);
        }

        public void Import(ParameterItem parameter)
        {
            name = parameter.name;
            saved = parameter.saved;
            defaultValue = parameter.defaultValue;
            defaultParameter |= parameter.defaultParameter;
            if (parameter.usages != null) usages.AddRange(parameter.usages);
        }

        public ParameterItem Export()
        {
            return new ParameterItem
            {
                name = name,
                saved = saved,
                defaultValue = defaultValue,
                valueKind = GuessValueKind(),
                defaultParameter = defaultParameter,
                usages = usages.Where(_ => !defaultParameter)
                    .Select(state => (valueKind: state.usageKind, state.value))
                    .Distinct()
                    .Select(usageValue => new ParameterUsage(usageValue.valueKind, usageValue.value))
                    .OrderBy(usage => usage.value)
                    .ToList()
            };
        }
    }
}