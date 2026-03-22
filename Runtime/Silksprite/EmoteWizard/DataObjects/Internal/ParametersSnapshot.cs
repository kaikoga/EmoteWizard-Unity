using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal.Builders;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public class ParametersSnapshot
    {
        [SerializeField] List<string> validReferenceUsages;

        [SerializeField] List<ParameterInstance> parameterItems;
        [SerializeField] List<ParameterInstance> implicitParameterItems;
        [SerializeField] List<ParameterInstance> defaultParameterItems;

        public IEnumerable<ParameterInstance> ParameterItems => parameterItems;
        public IEnumerable<ParameterInstance> ImplicitParameterItems => implicitParameterItems;
        public IEnumerable<ParameterInstance> DefaultParameterItems => defaultParameterItems;
        public IEnumerable<ParameterInstance> AllParameters => parameterItems.Concat(implicitParameterItems).Concat(defaultParameterItems);

        public ParametersSnapshot(List<ParameterInstance> parameterItems, List<ParameterInstance> implicitParameterItems, List<ParameterInstance> defaultParameterItems)
        {
            this.parameterItems = parameterItems;
            this.implicitParameterItems = implicitParameterItems;
            this.defaultParameterItems = defaultParameterItems;
            validReferenceUsages = AllParameters.SelectMany(item => item.referenceUsages).ToList();
        }

        public ParameterInstance? ResolveParameter(string parameterName)
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

        public bool IsInvalidParameterReference(string parameterReference)
        {
            return !string.IsNullOrEmpty(parameterReference) && !validReferenceUsages.Contains(parameterReference);
        }

        public static ParametersSnapshotBuilder Builder(EmoteWizardEnvironment env) => new ParametersSnapshotBuilder(env);
    }
}