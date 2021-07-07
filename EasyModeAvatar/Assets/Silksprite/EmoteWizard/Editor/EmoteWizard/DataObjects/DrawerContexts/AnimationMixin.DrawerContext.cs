using EmoteWizard.Base.DrawerContexts;

namespace EmoteWizard.DataObjects.DrawerContexts
{
    public class AnimationMixinDrawerContext : DrawerContextBase<AnimationMixinDrawerContext>
    {
        public readonly string RelativePath;

        public AnimationMixinDrawerContext() : base(null) { }
        public AnimationMixinDrawerContext(EmoteWizardRoot emoteWizardRoot, string relativePath) : base(emoteWizardRoot) => RelativePath = relativePath;
    }
}