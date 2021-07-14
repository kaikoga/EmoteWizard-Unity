using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class ParameterEmoteDrawerContext : EmoteWizardDrawerContextBase<ParameterEmote, ParameterEmoteDrawerContext>
    {
        public readonly AnimationWizardBase AnimationWizardBase;
        public readonly ParametersWizard ParametersWizard;
        public readonly string Layer;
        public readonly bool EditTargets;

        public ParameterEmoteDrawerContext() : base(null) { }
        public ParameterEmoteDrawerContext(EmoteWizardRoot emoteWizardRoot, AnimationWizardBase animationWizardBase, ParametersWizard parametersWizard, string layer, bool editTargets) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
            AnimationWizardBase = animationWizardBase;
            Layer = layer;
            EditTargets = editTargets;
        }
    }
}