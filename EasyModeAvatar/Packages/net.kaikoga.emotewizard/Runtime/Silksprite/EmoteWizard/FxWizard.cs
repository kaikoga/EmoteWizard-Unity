using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class FxWizard : AnimationWizardBase<IFxEmoteSource, IFxParameterEmoteSource, IFxAnimationMixinSource>
    {
        [SerializeField] public AnimationClip resetClip;

        [SerializeField] public string handSignOverrideParameter = EmoteWizardConstants.Defaults.Params.FxHandSignOverride;

        public override void DisconnectOutputAssets()
        {
            base.DisconnectOutputAssets();
            resetClip = null;
        }

        public override string LayerName => EmoteWizardConstants.LayerNames.Fx;
        public override string HandSignOverrideParameter => handSignOverrideParameter;
    }
}