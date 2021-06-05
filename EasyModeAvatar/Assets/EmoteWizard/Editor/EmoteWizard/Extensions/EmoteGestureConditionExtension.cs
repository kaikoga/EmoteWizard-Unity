using System;
using EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace EmoteWizard.Extensions
{
    public static class EmoteGestureConditionExtension
    {
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