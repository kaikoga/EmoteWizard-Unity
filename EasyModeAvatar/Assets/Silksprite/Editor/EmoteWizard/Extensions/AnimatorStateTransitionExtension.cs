using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class AnimatorStateTransitionExtension
    {
        public static void AddAlwaysTrueCondition(this AnimatorStateTransition transition)
        {
            transition.AddCondition(AnimatorConditionMode.Greater, -1, "Viseme");
        }
    }
}