using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class GestureLayerContext : AnimatorLayerContextBase
    {
        [UsedImplicitly]
        public GestureLayerContext(EmoteWizardEnvironment env) : base(env)
        {
            LayerKind = LayerKind.Gesture;
        }

        public GestureLayerContext(EmoteWizardEnvironment env, AnimatorLayerWizardBase wizard) : base(env, wizard) { }
    }
}