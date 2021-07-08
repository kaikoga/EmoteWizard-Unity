using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class ParameterEmoteDrawerContext : EmoteWizardDrawerContextBase<ParameterEmoteDrawerContext>
    {
        public readonly AnimationWizardBase AnimationWizardBase;
        public readonly string Layer;
        public readonly bool EditTargets;

        public ParameterEmoteDrawerContext() : base(null) { }
        public ParameterEmoteDrawerContext(EmoteWizardRoot emoteWizardRoot, AnimationWizardBase animationWizardBase, string layer, bool editTargets) : base(emoteWizardRoot)
        {
            AnimationWizardBase = animationWizardBase;
            Layer = layer;
            EditTargets = editTargets;
        }
    }
}