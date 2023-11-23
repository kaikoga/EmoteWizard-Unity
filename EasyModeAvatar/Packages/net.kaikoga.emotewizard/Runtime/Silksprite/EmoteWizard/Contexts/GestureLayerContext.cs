using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Contexts
{
    public class GestureLayerContext : AnimatorLayerContextBase
    {
        [UsedImplicitly]
        public GestureLayerContext(EmoteWizardEnvironment env) : base(env) { }
        public GestureLayerContext(EmoteWizardEnvironment env, AnimatorLayerWizardBase wizard) : base(env, wizard) { }
    }
}