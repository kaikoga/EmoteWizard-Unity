using Silksprite.EmoteWizard.Platforms.ChilloutVR.Internal.ConditionBuilders;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.ChilloutVR.Extensions
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