using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts
{
    public interface IAvatarWizardContext : IBehaviourContext
    {
        VRCAvatarDescriptor AvatarDescriptor { get; }
        Animator ProxyAnimator { get; set; }

        AvatarWizard.OverrideGeneratedControllerType2 OverrideGesture { get; }
        RuntimeAnimatorController OverrideGestureController { get; }
        AvatarWizard.OverrideGeneratedControllerType1 OverrideAction { get; }
        RuntimeAnimatorController OverrideActionController { get; }
        AvatarWizard.OverrideControllerType2 OverrideSitting { get; }
        RuntimeAnimatorController OverrideSittingController { get; }
    }
}