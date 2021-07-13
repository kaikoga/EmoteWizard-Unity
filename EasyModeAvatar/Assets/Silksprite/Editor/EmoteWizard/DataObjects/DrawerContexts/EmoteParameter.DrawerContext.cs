using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class EmoteParameterDrawerContext : EmoteWizardDrawerContextBase<EmoteParameterDrawerContext>
    {
        public readonly ParametersWizard ParametersWizard;
        public readonly bool IsEditing;

        public EmoteParameterDrawerContext() : base(null) { }
        public EmoteParameterDrawerContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard, bool isEditing) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
            IsEditing = isEditing;
        }
    }
}