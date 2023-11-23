using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class AvatarWizard : EmoteWizardBase, IAvatarWizardContext
    {
        [SerializeField] public VRCAvatarDescriptor avatarDescriptor;

        [SerializeField] public Animator proxyAnimator;

        [SerializeField] public OverrideGeneratedControllerType2 overrideGesture = OverrideGeneratedControllerType2.Generate;
        [SerializeField] public RuntimeAnimatorController overrideGestureController;
        [SerializeField] public OverrideGeneratedControllerType1 overrideAction = OverrideGeneratedControllerType1.Default;
        [SerializeField] public RuntimeAnimatorController overrideActionController;
        [SerializeField] public OverrideControllerType2 overrideSitting = OverrideControllerType2.Default2;
        [SerializeField] public RuntimeAnimatorController overrideSittingController;

        public override IBehaviourContext ToContext() => this;

        VRCAvatarDescriptor IAvatarWizardContext.AvatarDescriptor => avatarDescriptor;

        Animator IAvatarWizardContext.ProxyAnimator
        {
            get => proxyAnimator;
            set => proxyAnimator = value;
        }

        OverrideGeneratedControllerType2 IAvatarWizardContext.OverrideGesture => overrideGesture;

        RuntimeAnimatorController IAvatarWizardContext.OverrideGestureController => overrideGestureController;

        OverrideGeneratedControllerType1 IAvatarWizardContext.OverrideAction => overrideAction;

        RuntimeAnimatorController IAvatarWizardContext.OverrideActionController => overrideActionController;

        OverrideControllerType2 IAvatarWizardContext.OverrideSitting => overrideSitting;

        RuntimeAnimatorController IAvatarWizardContext.OverrideSittingController => overrideSittingController;
        
        Component IBehaviourContext.Component => this;

        public override void DisconnectOutputAssets()
        {
            avatarDescriptor = null;
            proxyAnimator = null;
        }

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