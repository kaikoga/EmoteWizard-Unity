using JetBrains.Annotations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Contexts
{
    public class AvatarContext : ContextBase<AvatarWizard>
    {
        VRCAvatarDescriptor _avatarDescriptor;
        public VRCAvatarDescriptor AvatarDescriptor
        {
            get => _avatarDescriptor;
            set
            {
                _avatarDescriptor = value;
                if (Wizard) Wizard.avatarDescriptor = value;
            }
        }

        Animator _proxyAnimator;
        public Animator ProxyAnimator
        {
            get => _proxyAnimator;
            set
            {
                _proxyAnimator = value;
                if (Wizard) Wizard.proxyAnimator = value;
            }
        }

        public readonly AvatarWizard.OverrideGeneratedControllerType2 OverrideGesture;
        public readonly RuntimeAnimatorController OverrideGestureController;
        public readonly AvatarWizard.OverrideGeneratedControllerType1 OverrideAction;
        public readonly RuntimeAnimatorController OverrideActionController;
        public readonly AvatarWizard.OverrideControllerType2 OverrideSitting;
        public readonly RuntimeAnimatorController OverrideSittingController;

        [UsedImplicitly]
        public AvatarContext(EmoteWizardEnvironment env) : base(env)
        {
            OverrideGesture = AvatarWizard.OverrideGeneratedControllerType2.Generate;
            OverrideAction = AvatarWizard.OverrideGeneratedControllerType1.Default;
            OverrideSitting = AvatarWizard.OverrideControllerType2.Default2;
        }

        public AvatarContext(EmoteWizardEnvironment env, AvatarWizard wizard) : base(env, wizard)
        {
            if (env.PersistGeneratedAssets)
            {
                _avatarDescriptor = wizard.avatarDescriptor;
                _proxyAnimator = wizard.proxyAnimator;
            }

            OverrideGesture = wizard.overrideGesture;
            OverrideGestureController = wizard.overrideGestureController;
            OverrideAction = wizard.overrideAction;
            OverrideActionController = wizard.overrideActionController;
            OverrideSitting = wizard.overrideSitting;
            OverrideSittingController = wizard.overrideSittingController;
        }

        public override void DisconnectOutputAssets()
        {
            AvatarDescriptor = null;
            ProxyAnimator = null;
        }
    }
}