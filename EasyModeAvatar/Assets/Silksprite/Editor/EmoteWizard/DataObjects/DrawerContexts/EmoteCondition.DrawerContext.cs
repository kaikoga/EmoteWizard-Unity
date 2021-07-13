using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class EmoteConditionDrawerContext : EmoteWizardDrawerContextBase<EmoteConditionDrawerContext>
    {
        public readonly ParametersWizard ParametersWizard;

        public EmoteConditionDrawerContext() : base(null) { }
        public EmoteConditionDrawerContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
        }
    }
}