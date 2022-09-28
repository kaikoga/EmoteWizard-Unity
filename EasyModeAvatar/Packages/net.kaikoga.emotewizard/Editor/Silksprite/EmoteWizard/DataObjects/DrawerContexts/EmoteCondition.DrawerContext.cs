using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    [UsedImplicitly]
    public class EmoteConditionDrawerContext : EmoteWizardDrawerContextBase<EmoteCondition, EmoteConditionDrawerContext>
    {
        public readonly ParametersWizard ParametersWizard;

        public EmoteConditionDrawerContext() : base(null) { }
        public EmoteConditionDrawerContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
        }
    }
}