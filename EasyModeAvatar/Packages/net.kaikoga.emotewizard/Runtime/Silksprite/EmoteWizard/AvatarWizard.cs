using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
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

        public override IBehaviourContext ToContext() => GetContext();
        public IAvatarWizardContext GetContext() => new AvatarContext(this);

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

        class AvatarContext : IAvatarWizardContext
        {
            readonly AvatarWizard _wizard;

            public AvatarContext(AvatarWizard wizard) => _wizard = wizard;

            IEmoteWizardEnvironment IBehaviourContext.Environment => _wizard.Environment;

            Component IBehaviourContext.Component => _wizard;

            VRCAvatarDescriptor IAvatarWizardContext.AvatarDescriptor
            {
                get => _wizard.avatarDescriptor;
                set => _wizard.avatarDescriptor = value;
            }

            Animator IAvatarWizardContext.ProxyAnimator
            {
                get => _wizard.proxyAnimator;
                set => _wizard.proxyAnimator = value;
            }

            OverrideGeneratedControllerType2 IAvatarWizardContext.OverrideGesture => _wizard.overrideGesture;

            RuntimeAnimatorController IAvatarWizardContext.OverrideGestureController => _wizard.overrideGestureController;

            OverrideGeneratedControllerType1 IAvatarWizardContext.OverrideAction => _wizard.overrideAction;

            RuntimeAnimatorController IAvatarWizardContext.OverrideActionController => _wizard.overrideActionController;

            OverrideControllerType2 IAvatarWizardContext.OverrideSitting => _wizard.overrideSitting;

            RuntimeAnimatorController IAvatarWizardContext.OverrideSittingController => _wizard.overrideSittingController;
        }
    }
}