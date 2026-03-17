using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Emote Wizard/Configs/Fx Layer Wizard Config", 10000)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/configs#fx-wizard-config")]
    public class FxLayerConfig : AnimatorLayerConfigBase
    {
        public override LayerOutputKind LayerOutputKind => LayerOutputKind.Fx;

        public override AnimatorLayerContextBase GetContext(EmoteWizardEnvironment env) => new FxLayerContext(env, this);

        protected override void Reset()
        {
            base.Reset();
            var context = new FxLayerContext(CreateEnv());
            hasResetClip = context.HasResetClip;
        }
    }
}