using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class EmoteDrawerContext : EmoteWizardDrawerContextBase<Emote, EmoteDrawerContext>
    {
        public readonly ParametersWizard ParametersWizard;
        public readonly bool AdvancedAnimations;

        public EmoteDrawerContext() : base(null) { }
        public EmoteDrawerContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard, bool advancedAnimations) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
            AdvancedAnimations = advancedAnimations;
        }
    }
}