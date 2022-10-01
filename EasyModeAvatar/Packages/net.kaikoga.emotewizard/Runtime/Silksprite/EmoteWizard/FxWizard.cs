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

        [SerializeField] public string handSignOverrideParameter = "EmoteWizardFX";

        public override void DisconnectOutputAssets()
        {
            base.DisconnectOutputAssets();
            resetClip = null;
        }

        public override string LayerName => "FX";
        public override string HandSignOverrideParameter => handSignOverrideParameter;
    }
}