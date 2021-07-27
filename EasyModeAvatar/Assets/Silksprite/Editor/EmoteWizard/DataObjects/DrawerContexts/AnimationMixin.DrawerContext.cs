using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    [UsedImplicitly]
    public class AnimationMixinDrawerContext : EmoteWizardDrawerContextBase<AnimationMixin, AnimationMixinDrawerContext>
    {
        public readonly string RelativePath;

        public AnimationMixinDrawerContext() : base(null) { }
        public AnimationMixinDrawerContext(EmoteWizardRoot emoteWizardRoot, string relativePath) : base(emoteWizardRoot) => RelativePath = relativePath;
    }
}