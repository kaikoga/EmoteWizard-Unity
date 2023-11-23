using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts
{
    public class AvatarContext : IBehaviourContext
    {
        readonly AvatarWizard _wizard;

        public AvatarContext(AvatarWizard wizard) => _wizard = wizard;

        public IEmoteWizardEnvironment Environment => _wizard.Environment;

        public GameObject GameObject => _wizard.gameObject;

        public VRCAvatarDescriptor AvatarDescriptor
        {
            get => _wizard.avatarDescriptor;
            set => _wizard.avatarDescriptor = value;
        }

        public Animator ProxyAnimator
        {
            get => _wizard.proxyAnimator;
            set => _wizard.proxyAnimator = value;
        }

        public void DisconnectOutputAssets()
        {
            _wizard.avatarDescriptor = null;
            _wizard.proxyAnimator = null;
        }

        public AvatarWizard.OverrideGeneratedControllerType2 OverrideGesture => _wizard.overrideGesture;

        public RuntimeAnimatorController OverrideGestureController => _wizard.overrideGestureController;

        public AvatarWizard.OverrideGeneratedControllerType1 OverrideAction => _wizard.overrideAction;

        public RuntimeAnimatorController OverrideActionController => _wizard.overrideActionController;

        public AvatarWizard.OverrideControllerType2 OverrideSitting => _wizard.overrideSitting;

        public RuntimeAnimatorController OverrideSittingController => _wizard.overrideSittingController;
    }
}