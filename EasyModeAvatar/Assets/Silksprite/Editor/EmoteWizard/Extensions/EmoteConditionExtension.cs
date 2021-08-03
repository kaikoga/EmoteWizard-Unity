using System;
using Silksprite.EmoteWizard.DataObjects;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class EmoteConditionExtension
    {
        public static AnimatorConditionMode ToAnimatorConditionMode(this EmoteCondition emoteCondition)
        {
            switch (emoteCondition.mode)
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