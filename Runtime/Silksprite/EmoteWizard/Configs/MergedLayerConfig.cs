using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Emote Wizard/Configs/Merged Layer Wizard Config", 10100)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/configs#merged-wizard-config")]
    public class MergedLayerConfig : AnimatorControllerConfigBase
    {
        public override LayerOutputKind LayerOutputKind => LayerOutputKind.Merged;

        public override AnimatorControllerContextBase GetContext(EmoteWizardEnvironment env) => new MergedLayerContext(env, this);

        protected override void Reset()
        {
            base.Reset();
            var context = new MergedLayerContext(CreateEnv());
            hasResetClip = context.HasResetClip;
        }
    }
}