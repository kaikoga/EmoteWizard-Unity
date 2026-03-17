using System;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Extensions;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Internal.ConditionBuilders
{
    public static class ConditionBuilderEmoteWizardExtension
    {
        public static ConditionBuilder EmoteCondition(this ConditionBuilder builder, EmoteConditionInstance emoteCondition, ParameterValueKind? actualValueKind)
        {
            AnimatorControllerParameterType type;
            AnimatorConditionMode mode;
            switch (actualValueKind)
            {
                case ParameterValueKind.Bool:
                    type = AnimatorControllerParameterType.Bool;
                    switch (emoteCondition.Mode)
                    {
                        case EmoteConditionMode.If:
                            mode = AnimatorConditionMode.If;
                            break;
                        case EmoteConditionMode.IfNot:
                            mode = AnimatorConditionMode.IfNot;
                            break;
                        case EmoteConditionMode.Greater:
                            mode = AnimatorConditionMode.IfNot;
                            break;
                        case EmoteConditionMode.Less:
                            mode = AnimatorConditionMode.IfNot;
                            break;
                        case EmoteConditionMode.Equals:
                            mode = emoteCondition.Threshold != 0 ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot;
                            break;
                        case EmoteConditionMode.NotEqual:
                            mode = emoteCondition.Threshold != 0 ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    switch (emoteCondition.Mode)
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
                            type = actualValueKind == ParameterValueKind.Int ? AnimatorControllerParameterType.Int : AnimatorControllerParameterType.Float;
                            mode = AnimatorConditionMode.Greater;
                            break;
                        case EmoteConditionMode.Less:
                            type = actualValueKind == ParameterValueKind.Int ? AnimatorControllerParameterType.Int : AnimatorControllerParameterType.Float;
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
                    break;
            }

            builder.AddCondition(type, mode, emoteCondition.Parameter, emoteCondition.Threshold);
            return builder;
        }

        public static ConditionBuilder AlwaysTrue(this ConditionBuilder builder, EmoteWizardEnvironment env)
        {
            var platformFeatures = env.GetPlatformFeatures();
            builder.AddCondition(AnimatorControllerParameterType.Int, AnimatorConditionMode.Greater, platformFeatures.ParameterForAlwaysTrue, -1);
            return builder;
        }
    }
}