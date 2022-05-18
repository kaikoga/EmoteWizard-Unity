using Silksprite.EmoteWizard.Internal.ConditionBuilders;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Extensions
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