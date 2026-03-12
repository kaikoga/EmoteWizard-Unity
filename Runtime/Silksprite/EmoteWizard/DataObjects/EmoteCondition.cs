using System;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.DataObjects.Platforms;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteCondition
    {
        [SerializeField] public ParameterItemKind kind = ParameterItemKind.Auto;
        [ParameterName(false, false)]
        [SerializeField] public string parameter;
        [SerializeField] public EmoteConditionMode mode = EmoteConditionMode.Equals;
        [SerializeField] public float threshold;
        
        public EmoteConditionInstance ToInstance(EmoteWizardEnvironment environment)
        {
            var platformFeatures = PlatformFeatures.Of(environment);
            return new EmoteConditionInstance(kind, parameter, mode,
                platformFeatures.IsHandSignParameterReference(parameter) ? platformFeatures.HandSignValue((HandSign)threshold) : threshold);
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