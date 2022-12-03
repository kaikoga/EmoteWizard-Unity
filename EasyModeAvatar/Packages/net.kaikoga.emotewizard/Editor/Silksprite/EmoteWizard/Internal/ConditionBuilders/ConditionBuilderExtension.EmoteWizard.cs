using System;
using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Internal.ConditionBuilders
{
    public static class ConditionBuilderEmoteWizardExtension
    {
        public static ConditionBuilder EmoteCondition(this ConditionBuilder builder, EmoteCondition emoteCondition, ParameterItemKind actualItemKind)
        {
            AnimatorControllerParameterType type;
            AnimatorConditionMode mode;
            switch (actualItemKind)
            {
                case ParameterItemKind.Bool:
                    type = AnimatorControllerParameterType.Bool;
                    switch (emoteCondition.mode)
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
                            mode = AnimatorConditionMode.If;
                            break;
                        case EmoteConditionMode.NotEqual:
                            mode = AnimatorConditionMode.IfNot;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
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
                            type = actualItemKind == ParameterItemKind.Int ? AnimatorControllerParameterType.Int : AnimatorControllerParameterType.Float;
                            mode = AnimatorConditionMode.Greater;
                            break;
                        case EmoteConditionMode.Less:
                            type = actualItemKind == ParameterItemKind.Int ? AnimatorControllerParameterType.Int : AnimatorControllerParameterType.Float;
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

            builder.AddCondition(type, mode, emoteCondition.parameter, emoteCondition.threshold);
            return builder;
        }

        public static ConditionBuilder AlwaysTrue(this ConditionBuilder builder)
        {
            builder.AddCondition(AnimatorControllerParameterType.Int, AnimatorConditionMode.Greater, EmoteWizardConstants.Params.Viseme, -1);
            return builder;
        }
    }
}