using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ActionLayerContext : AnimatorLayerContextBase
    {
        [UsedImplicitly]
        public ActionLayerContext(EmoteWizardEnvironment env) : base(env) { }
        public ActionLayerContext(EmoteWizardEnvironment env, AnimatorLayerWizardBase wizard) : base(env, wizard) { }
    }
}