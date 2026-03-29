using System;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteCondition
    {
        [SerializeField] public ParameterItemKind kind = ParameterItemKind.Auto;
        [ParameterName(false, false)]
        [SerializeField] public string parameter = "";
        [SerializeField] public EmoteConditionMode mode = EmoteConditionMode.Equals;
        [SerializeField] public float threshold;

        public (string resolvedParameter, float resolvedThreshold) ResolveParameter(IPlatformFeatures platformFeatures)
        {
            var resolvedParameter = platformFeatures.ResolveParameterReference(parameter);
            var resolvedThreshold = platformFeatures.IsHandSignParameterReference(parameter) ? platformFeatures.HandSignValue((HandSign)threshold) : threshold;
            return (resolvedParameter, resolvedThreshold);
        }

        public EmoteConditionInstance ToInstance(IPlatformFeatures platformFeatures)
        {
            var (resolvedParameter, resolvedThreshold) = ResolveParameter(platformFeatures);
            return new EmoteConditionInstance(
                kind,
                resolvedParameter,
                mode,
                resolvedThreshold,
                ParameterValue.Create(kind, threshold));
        }
    }

    public enum EmoteConditionMode
    {
        If = 1,
        IfNot = 2,
        Greater = 3,
        Less = 4,
        Equals = 6,
        NotEqual = 7,
    }
}