using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ActionLayerContext : AnimatorLayerContextBase
    {
        [UsedImplicitly]
        public ActionLayerContext(EmoteWizardEnvironment env) : base(env)
        {
            LayerKind = LayerKind.Action;
        }

        public ActionLayerContext(EmoteWizardEnvironment env, AnimatorLayerWizardBase wizard) : base(env, wizard) { }
    }
}