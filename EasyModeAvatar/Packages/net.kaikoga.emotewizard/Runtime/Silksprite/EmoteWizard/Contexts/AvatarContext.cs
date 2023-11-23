using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts
{
    public class AvatarContext : IAvatarWizardContext
    {
        readonly AvatarWizard _wizard;

        public AvatarContext(AvatarWizard wizard) => _wizard = wizard;

        IEmoteWizardEnvironment IBehaviourContext.Environment => _wizard.Environment;

        GameObject IBehaviourContext.GameObject => _wizard.gameObject;

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

        void IBehaviourContext.DisconnectOutputAssets()
        {
            _wizard.avatarDescriptor = null;
            _wizard.proxyAnimator = null;
        }

        AvatarWizard.OverrideGeneratedControllerType2 IAvatarWizardContext.OverrideGesture => _wizard.overrideGesture;

        RuntimeAnimatorController IAvatarWizardContext.OverrideGestureController => _wizard.overrideGestureController;

        AvatarWizard.OverrideGeneratedControllerType1 IAvatarWizardContext.OverrideAction => _wizard.overrideAction;

        RuntimeAnimatorController IAvatarWizardContext.OverrideActionController => _wizard.overrideActionController;

        AvatarWizard.OverrideControllerType2 IAvatarWizardContext.OverrideSitting => _wizard.overrideSitting;

        RuntimeAnimatorController IAvatarWizardContext.OverrideSittingController => _wizard.overrideSittingController;
    }
}