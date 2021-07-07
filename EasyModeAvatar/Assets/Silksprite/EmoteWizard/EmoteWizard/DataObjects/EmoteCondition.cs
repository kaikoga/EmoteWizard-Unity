using System;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteCondition
    {
        [SerializeField] public string parameter;
        [SerializeField] public EmoteConditionMode mode;
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