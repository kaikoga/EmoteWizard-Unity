using System;
using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.ConditionBuilders
{
    public static class ConditionBuilderEmoteWizardExtension
    {
        [Obsolete("EmoteCondition should be typed")]
        public static ConditionBuilder EmoteCondition(this ConditionBuilder builder, EmoteCondition emoteCondition)
        {
            AnimatorControllerParameterType type;
            AnimatorConditionMode mode;
            switch (emoteCondition.mode)
            {
                case EmoteConditionMode.If:
                    type = AnimatorControllerParameterType.Bool;
                    mode = AnimatorConditionMode.If;
                    break;
                case EmoteConditionMode.IfNot:
                    type = AnimatorControllerParameterType.Bool;
                    mode = AnimatorConditionMode.IfNot;
                    break;
                case EmoteConditionMode.Greater:
                    type = AnimatorControllerParameterType.Float;
                    mode = AnimatorConditionMode.Greater;
                    break;
                case EmoteConditionMode.Less:
                    type = AnimatorControllerParameterType.Float;
                    mode = AnimatorConditionMode.Less;
                    break;
                case EmoteConditionMode.Equals:
                    type = AnimatorControllerParameterType.Int;
                    mode = AnimatorConditionMode.Equals;
                    break;
                case EmoteConditionMode.NotEqual:
                    type = AnimatorControllerParameterType.Int;
                    mode = AnimatorConditionMode.NotEqual;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            builder.AddCondition(type, mode, emoteCondition.parameter, emoteCondition.threshold);
            return builder;
        }

        public static ConditionBuilder AlwaysTrue(this ConditionBuilder builder)
        {
            builder.AddCondition(AnimatorControllerParameterType.Int, AnimatorConditionMode.Greater, "Viseme", -1);
            return builder;
        }
    }
}