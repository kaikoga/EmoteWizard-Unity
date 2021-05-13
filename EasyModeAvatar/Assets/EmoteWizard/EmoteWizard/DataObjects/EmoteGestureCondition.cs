using System;
using UnityEditor.Animations;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteGestureCondition
    {
        [SerializeField] public GestureParameter parameter;
        [SerializeField] public GestureConditionMode mode;
        [SerializeField] public EmoteHandSign handSign;

        public static EmoteGestureCondition Populate(EmoteHandSign handSign, GestureParameter parameter, GestureConditionMode mode = GestureConditionMode.Equals)
        {
            return new EmoteGestureCondition
            {
                parameter = parameter,
                mode = mode,
                handSign = handSign
            };
        }

        public AnimatorConditionMode ResolvedMode
        {
            get {
                switch (mode)
                {
                    case GestureConditionMode.Equals:
                        return AnimatorConditionMode.Equals;
                    case GestureConditionMode.NotEqual:
                        return AnimatorConditionMode.NotEqual;
                    case GestureConditionMode.Ignore:
                        return AnimatorConditionMode.Equals;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public float ResolvedThreshold => (int) handSign;
    }

    public enum GestureParameter
    {
        Gesture,
        GestureOther,
        GestureLeft,
        GestureRight,
    }

    public enum GestureConditionMode
    {
        Equals,
        NotEqual,
        Ignore
    }
}