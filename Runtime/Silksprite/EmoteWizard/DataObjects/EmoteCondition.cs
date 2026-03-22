using System;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Extensions;
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

        public (string resolvedParameter, float resolvedThreshold) ResolveParameter(EmoteWizardEnvironment environment)
        {
            var platformFeatures = environment.GetPlatformFeatures();
            var resolvedParameter = platformFeatures.ResolveParameterReference(parameter);
            var resolvedThreshold = platformFeatures.IsHandSignParameterReference(parameter) ? platformFeatures.HandSignValue((HandSign)threshold) : threshold;
            return (resolvedParameter, resolvedThreshold);
        }

        public EmoteConditionInstance ToInstance(EmoteWizardEnvironment environment)
        {
            var (resolvedParameter, resolvedThreshold) = ResolveParameter(environment);
            return new EmoteConditionInstance(
                kind,
                resolvedParameter,
                mode,
                resolvedThreshold);
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