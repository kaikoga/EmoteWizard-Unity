using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    [DisallowMultipleComponent]
    public class FxLayerConfig : AnimatorLayerConfigBase
    {
        public override LayerKind LayerKind => LayerKind.FX;

        public override AnimatorLayerContextBase GetContext(EmoteWizardEnvironment env) => new FxLayerContext(env, this);

        protected override void Reset()
        {
            base.Reset();
            var context = new FxLayerContext(CreateEnv());
            hasResetClip = context.HasResetClip;
        }
    }
}