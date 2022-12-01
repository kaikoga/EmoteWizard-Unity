using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Legacy
{
    [Serializable]
    public class EmoteGestureCondition
    {
        [SerializeField] public GestureParameter parameter;
        [SerializeField] public GestureConditionMode mode;
        [SerializeField] public HandSign handSign;

        public static EmoteGestureCondition Populate(HandSign handSign, GestureParameter parameter, GestureConditionMode mode = GestureConditionMode.Equals)
        {
            return new EmoteGestureCondition
            {
                parameter = parameter,
                mode = mode,
                handSign = handSign
            };
        }
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