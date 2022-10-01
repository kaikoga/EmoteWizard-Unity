using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class GestureWizard : AnimationWizardBase<IGestureEmoteSource, IGestureParameterEmoteSource, IGestureAnimationMixinSource>
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public string handSignOverrideParameter = EmoteWizardConstants.Defaults.Params.GestureHandSignOverride;

        public override string LayerName => EmoteWizardConstants.LayerNames.Gesture;
        public override string HandSignOverrideParameter => handSignOverrideParameter;
    }
}