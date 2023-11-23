using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Contexts
{
    public class FxLayerContext : AnimatorLayerContextBase
    {
        [UsedImplicitly]
        public FxLayerContext(EmoteWizardEnvironment env) : base(env) { }
        public FxLayerContext(EmoteWizardEnvironment env, AnimatorLayerWizardBase wizard) : base(env, wizard) { }
    }
}