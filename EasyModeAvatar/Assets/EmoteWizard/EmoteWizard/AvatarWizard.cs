using EmoteWizard.Base;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class AvatarWizard : EmoteWizardBase
    {
        [SerializeField] public VRCAvatarDescriptor avatarDescriptor;

        [SerializeField] public Animator proxyAnimator;

        [SerializeField] public OverrideGeneratedControllerType2 overrideGesture = OverrideGeneratedControllerType2.Generate;
        [SerializeField] public RuntimeAnimatorController overrideGestureController;
        [SerializeField] public OverrideControllerType2 overrideSitting = OverrideControllerType2.Default2;
        [SerializeField] public RuntimeAnimatorController overrideSittingController;
        
        public enum OverrideGeneratedControllerType2
        {
            Generate,
            Override,
            Default1,
            Default2
        }
        
        public enum OverrideControllerType2
        {
            Override,
            Default1,
            Default2
        }
    }
}