using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class MergedLayerContext : AnimatorControllerContextBase
    {
        [UsedImplicitly]
        public MergedLayerContext(EmoteWizardEnvironment env) : base(env)
        {
            LayerOutputKind = LayerOutputKind.Merged;
            HasResetClip = true;
        }

        public MergedLayerContext(EmoteWizardEnvironment env, MergedLayerConfig config) : base(env, config) { }
    }
}