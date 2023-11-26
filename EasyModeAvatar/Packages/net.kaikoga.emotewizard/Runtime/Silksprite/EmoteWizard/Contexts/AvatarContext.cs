using JetBrains.Annotations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public class AvatarContext : ContextBase<AvatarWizard>
    {
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
            OverrideGesture = wizard.overrideGesture;
            OverrideGestureController = wizard.overrideGestureController;
            OverrideAction = wizard.overrideAction;
            OverrideActionController = wizard.overrideActionController;
            OverrideSitting = wizard.overrideSitting;
            OverrideSittingController = wizard.overrideSittingController;
        }

        public override void DisconnectOutputAssets()
        {
        }
    }
}