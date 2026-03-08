using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal.Builders;
using Silksprite.EmoteWizard.DataObjects.Platforms;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public class ParametersSnapshot
    {
        [SerializeField] public List<ParameterInstance> parameterItems;
        [SerializeField] public List<ParameterInstance> implicitParameterItems;
        static readonly List<ParameterInstance> DefaultParameterItems = DefaultParameters.Populate();

        public IEnumerable<ParameterInstance> AllParameters => parameterItems.Concat(implicitParameterItems).Concat(DefaultParameterItems);

        public ParameterInstance ResolveParameter(string parameterName)
        {
            return AllParameters.FirstOrDefault(item => item.name == parameterName);
        }

        public ParameterValueKind? ResolveParameterType(string parameterName, ParameterItemKind itemKind, out bool mismatch)
        {
            mismatch = false;
            var item = ResolveParameter(parameterName);
            if (item == null) return null;

            var resolvedValueKind = item.ValueKind;
            switch (resolvedValueKind)
            {
                case ParameterValueKind.Bool:
                    if (itemKind == ParameterItemKind.Auto || itemKind == ParameterItemKind.Bool) return resolvedValueKind;
                    break;
                case ParameterValueKind.Int:
                    if (itemKind == ParameterItemKind.Auto || itemKind == ParameterItemKind.Int) return resolvedValueKind;
                    break;
                case ParameterValueKind.Float:
                    if (itemKind == ParameterItemKind.Auto || itemKind == ParameterItemKind.Float) return resolvedValueKind;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            mismatch = true;
            return resolvedValueKind;
        }

        public bool IsInvalidParameter(string parameterName)
        {
            return !string.IsNullOrEmpty(parameterName) && AllParameters.All(item => item.name != parameterName);
        }

        public static ParametersSnapshotBuilder Builder() => new ParametersSnapshotBuilder();
    }
}