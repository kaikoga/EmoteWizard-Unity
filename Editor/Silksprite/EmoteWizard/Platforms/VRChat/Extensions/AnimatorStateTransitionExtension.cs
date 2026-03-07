using Silksprite.EmoteWizard.Platforms.VRChat.Internal.ConditionBuilders;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Extensions
{
    public static class AnimatorStateTransitionExtension
    {
        public static void AddCondition(this AnimatorStateTransition transition, ConditionBuilder builder)
        {
            foreach (var cond in builder.ToArray())
            {
                transition.AddCondition(cond.mode, cond.threshold, cond.parameter);
            }
        }
    }
}