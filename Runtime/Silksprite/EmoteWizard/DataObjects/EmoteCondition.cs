using System;
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