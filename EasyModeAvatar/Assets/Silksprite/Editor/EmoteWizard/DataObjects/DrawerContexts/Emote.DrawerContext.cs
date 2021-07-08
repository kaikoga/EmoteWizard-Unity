using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class EmoteDrawerContext : EmoteWizardDrawerContextBase<EmoteDrawerContext>
    {
        public readonly bool AdvancedAnimations;

        public EmoteDrawerContext() : base(null) { }
        public EmoteDrawerContext(EmoteWizardRoot emoteWizardRoot, bool advancedAnimations) : base(emoteWizardRoot) => AdvancedAnimations = advancedAnimations;
    }
}