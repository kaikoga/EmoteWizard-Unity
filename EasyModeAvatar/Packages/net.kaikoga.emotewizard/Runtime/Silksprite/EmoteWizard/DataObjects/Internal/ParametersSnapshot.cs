using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParametersSnapshot
    {
        public List<ParameterInstance> ParameterItems;
        static readonly List<ParameterInstance> DefaultParameterItems = DefaultParameters.Populate();

        public IEnumerable<ParameterInstance> AllParameters => ParameterItems.Concat(DefaultParameterItems);

        public ParameterInstance ResolveParameter(string parameterName)
        {
            foreach (var item in AllParameters)
            {
                if (item.Name != parameterName) continue;
                return item;
            }

            Debug.LogWarning($"Possibly not found parameter: {parameterName}");
            return null;
        }

        public ParameterValueKind? ResolveParameterType(string parameterName, ParameterItemKind itemKind)
        {
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
            Debug.LogWarning($"Possibly parameter type mismatch: {parameterName}, expected {itemKind}, was {resolvedValueKind}");
            return resolvedValueKind;
        }

        public VRCExpressionParameters.Parameter[] ToParameters()
        {
            return ParameterItems
                .Select(parameter => parameter.ToParameter())
                .ToArray();
        }

        public bool IsInvalidParameter(string parameterName)
        {
            return !string.IsNullOrEmpty(parameterName) && AllParameters.All(item => item.Name != parameterName);
        }
    }
}