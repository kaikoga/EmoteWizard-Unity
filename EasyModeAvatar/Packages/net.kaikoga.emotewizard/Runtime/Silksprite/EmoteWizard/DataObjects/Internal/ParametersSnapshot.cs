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

        public ParameterValueKind? ResolveParameterType(string parameterName, ParameterItemKind itemKind)
        {
            foreach (var item in AllParameters)
            {
                if (item.Name != parameterName) continue;

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

            Debug.LogWarning($"Possibly not found parameter: {parameterName} ({itemKind})");
            return null;
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