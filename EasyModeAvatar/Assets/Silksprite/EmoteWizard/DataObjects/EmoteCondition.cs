using System;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteCondition
    {
        [SerializeField] public string parameter;
        [SerializeField] public EmoteConditionMode mode;
        [SerializeField] public float threshold;

        public AnimatorConditionMode AnimatorConditionMode
        {
            get
            {
                switch (mode)
                {
                    case EmoteConditionMode.If:
                        return AnimatorConditionMode.If;
                    case EmoteConditionMode.IfNot:
                        return AnimatorConditionMode.IfNot;
                    case EmoteConditionMode.Greater:
                        return AnimatorConditionMode.Greater;
                    case EmoteConditionMode.Less:
                        return AnimatorConditionMode.Less;
                    case EmoteConditionMode.Equals:
                        return AnimatorConditionMode.Equals;
                    case EmoteConditionMode.NotEqual:
                        return AnimatorConditionMode.NotEqual;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
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