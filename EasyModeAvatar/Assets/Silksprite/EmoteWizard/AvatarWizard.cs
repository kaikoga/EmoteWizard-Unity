using Silksprite.EmoteWizard.Base;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class AvatarWizard : EmoteWizardBase
    {
        [SerializeField] public VRCAvatarDescriptor avatarDescriptor;

        [SerializeField] public Animator proxyAnimator;

        [SerializeField] public OverrideGeneratedControllerType2 overrideGesture = OverrideGeneratedControllerType2.Generate;
        [SerializeField] public RuntimeAnimatorController overrideGestureController;
        [SerializeField] public OverrideGeneratedControllerType1 overrideAction = OverrideGeneratedControllerType1.Default;
        [SerializeField] public RuntimeAnimatorController overrideActionController;
        [SerializeField] public OverrideControllerType2 overrideSitting = OverrideControllerType2.Default2;
        [SerializeField] public RuntimeAnimatorController overrideSittingController;
        
        public enum OverrideGeneratedControllerType1
        {
            Generate = 0x10,
            Override = 0x11,
            Default = 0x00,
        }
        
        public enum OverrideGeneratedControllerType2
        {
            Generate = 0x10,
            Override = 0x11,
            Default1 = 0x00,
            Default2 = 0x01
        }
        
        public enum OverrideControllerType2
        {
            Override = 0x11,
            Default1 = 0x00,
            Default2 = 0x01
        }
    }
}