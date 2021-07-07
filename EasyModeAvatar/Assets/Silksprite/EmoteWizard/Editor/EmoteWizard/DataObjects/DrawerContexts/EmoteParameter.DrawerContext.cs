using EmoteWizard.Base.DrawerContexts;

namespace EmoteWizard.DataObjects.DrawerContexts
{
    public class EmoteParameterDrawerContext : DrawerContextBase<EmoteParameterDrawerContext>
    {
        public readonly bool IsEditing;

        public EmoteParameterDrawerContext() : base(null) { }
        public EmoteParameterDrawerContext(EmoteWizardRoot emoteWizardRoot, bool isEditing) : base(emoteWizardRoot) => IsEditing = isEditing;
    }
}