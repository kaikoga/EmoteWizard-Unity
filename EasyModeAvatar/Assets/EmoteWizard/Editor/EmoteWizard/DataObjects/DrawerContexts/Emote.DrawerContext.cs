using EmoteWizard.Base.DrawerContexts;

namespace EmoteWizard.DataObjects.DrawerContexts
{
    public class EmoteDrawerContext : DrawerContextBase<EmoteDrawerContext>
    {
        public readonly bool AdvancedAnimations;

        public EmoteDrawerContext() : base(null) { }
        public EmoteDrawerContext(EmoteWizardRoot emoteWizardRoot, bool advancedAnimations) : base(emoteWizardRoot) => AdvancedAnimations = advancedAnimations;
    }
}