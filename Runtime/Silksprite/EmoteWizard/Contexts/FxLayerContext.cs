using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class FxLayerContext : AnimatorLayerContextBase
    {
        [UsedImplicitly]
        public FxLayerContext(EmoteWizardEnvironment env) : base(env)
        {
            LayerKind = LayerKind.FX;
            HasResetClip = true;
        }

        public FxLayerContext(EmoteWizardEnvironment env, AnimatorLayerConfigBase config) : base(env, config) { }
    }
}