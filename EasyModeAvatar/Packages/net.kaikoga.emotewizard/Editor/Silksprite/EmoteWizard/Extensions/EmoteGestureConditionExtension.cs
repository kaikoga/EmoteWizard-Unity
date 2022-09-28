using System;
using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class EmoteGestureConditionExtension
    {
        const string GestureLeft = "GestureLeft";
        const string GestureRight = "GestureRight";

        public static string ResolveParameter(this EmoteGestureCondition condition, bool isLeft)
        {
            switch (condition.parameter)
            {
                case GestureParameter.Gesture:
                    return isLeft ? GestureLeft : GestureRight;
                case GestureParameter.GestureOther:
                    return isLeft ? GestureRight : GestureLeft;
                case GestureParameter.GestureLeft:
                    return GestureLeft;
                case GestureParameter.GestureRight:
                    return GestureRight;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static AnimatorConditionMode ResolveMode(this EmoteGestureCondition condition)
        {
            switch (condition.mode)
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
        
        public static float ResolveThreshold(this EmoteGestureCondition condition) => (int) condition.handSign;
    }
}