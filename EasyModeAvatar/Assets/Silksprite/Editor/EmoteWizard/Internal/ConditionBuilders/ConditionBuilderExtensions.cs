using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.ConditionBuilders
{
    public static class ConditionBuilderExtension
    {
        const float FLOAT_TOLERANCE = 0.05f;

        public static ConditionBuilder If(this ConditionBuilder builder, string paramName, bool value)
        {
            builder.AddCondition(AnimatorControllerParameterType.Bool, value ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot, paramName, 0f);
            return builder;
        }

        public static ConditionBuilder NotEqual(this ConditionBuilder builder, string paramName, int value)
        {
            builder.AddCondition(AnimatorControllerParameterType.Int, AnimatorConditionMode.NotEqual, paramName, value);
            return builder;
        }

        public static ConditionBuilder Equals(this ConditionBuilder builder, string paramName, int value)
        {
            builder.AddCondition(AnimatorControllerParameterType.Int, AnimatorConditionMode.Equals, paramName, value);
            return builder;
        }

        public static ConditionBuilder Approximate(this ConditionBuilder builder, string paramName, float value)
        {
            return InclusiveRange(builder, paramName, value, value);
        }

        public static ConditionBuilder InclusiveRange(this ConditionBuilder builder, string paramName, int? min, int? max)
        {
            return builder.Range(AnimatorControllerParameterType.Int, paramName, min, max, 1f);
        }

        public static ConditionBuilder InclusiveRange(this ConditionBuilder builder, string paramName, float? min, float? max)
        {
            return builder.Range(AnimatorControllerParameterType.Float, paramName, min, max, FLOAT_TOLERANCE);
        }

        public static ConditionBuilder ExclusiveRange(this ConditionBuilder builder, string paramName, int? min, int? max)
        {
            return builder.Range(AnimatorControllerParameterType.Int, paramName, min, max, 0f);
        }

        public static ConditionBuilder ExclusiveRange(this ConditionBuilder builder, string paramName, float? min, float? max)
        {
            return builder.Range(AnimatorControllerParameterType.Float, paramName, min, max, -FLOAT_TOLERANCE);
        }

        static ConditionBuilder Range(this ConditionBuilder builder, AnimatorControllerParameterType paramType, string paramName, float? min, float? max, float tolerance)
        {
            if (min is float minValue)
            {
                builder.AddCondition(paramType, AnimatorConditionMode.Greater, paramName, minValue - tolerance);
            }
            if (max is float maxValue)
            {
                builder.AddCondition(paramType, AnimatorConditionMode.Less, paramName, maxValue + tolerance);
            }
            return builder;
        }
    }
}