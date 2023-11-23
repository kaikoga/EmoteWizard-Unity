using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts
{
    public class AvatarContext : ContextBase<AvatarWizard>
    {
        public AvatarContext(AvatarWizard wizard) : base(wizard) { }

        public VRCAvatarDescriptor AvatarDescriptor
        {
            get => Wizard.avatarDescriptor;
            set => Wizard.avatarDescriptor = value;
        }

        public Animator ProxyAnimator
        {
            get => Wizard.proxyAnimator;
            set => Wizard.proxyAnimator = value;
        }

        public override void DisconnectOutputAssets()
        {
            Wizard.avatarDescriptor = null;
            Wizard.proxyAnimator = null;
        }

        public AvatarWizard.OverrideGeneratedControllerType2 OverrideGesture => Wizard.overrideGesture;

        public RuntimeAnimatorController OverrideGestureController => Wizard.overrideGestureController;

        public AvatarWizard.OverrideGeneratedControllerType1 OverrideAction => Wizard.overrideAction;

        public RuntimeAnimatorController OverrideActionController => Wizard.overrideActionController;

        public AvatarWizard.OverrideControllerType2 OverrideSitting => Wizard.overrideSitting;

        public RuntimeAnimatorController OverrideSittingController => Wizard.overrideSittingController;
    }
}