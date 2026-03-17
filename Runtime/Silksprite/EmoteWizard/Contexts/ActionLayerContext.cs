using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ActionLayerContext : AnimatorLayerContextBase
    {
        [UsedImplicitly]
        public ActionLayerContext(EmoteWizardEnvironment env) : base(env)
        {
            LayerOutputKind = LayerOutputKind.Action;
        }

        public ActionLayerContext(EmoteWizardEnvironment env, AnimatorLayerConfigBase config) : base(env, config) { }
    }
}