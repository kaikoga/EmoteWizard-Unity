using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class AnimationMixinDrawerContext : DrawerContextBase<AnimationMixinDrawerContext>
    {
        public readonly string RelativePath;

        public AnimationMixinDrawerContext() : base(null) { }
        public AnimationMixinDrawerContext(EmoteWizardRoot emoteWizardRoot, string relativePath) : base(emoteWizardRoot) => RelativePath = relativePath;
    }
}