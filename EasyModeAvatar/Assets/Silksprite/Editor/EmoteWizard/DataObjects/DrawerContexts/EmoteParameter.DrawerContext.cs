using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class EmoteParameterDrawerContext : EmoteWizardDrawerContextBase<EmoteParameterDrawerContext>
    {
        public readonly bool IsEditing;

        public EmoteParameterDrawerContext() : base(null) { }
        public EmoteParameterDrawerContext(EmoteWizardRoot emoteWizardRoot, bool isEditing) : base(emoteWizardRoot) => IsEditing = isEditing;
    }
}