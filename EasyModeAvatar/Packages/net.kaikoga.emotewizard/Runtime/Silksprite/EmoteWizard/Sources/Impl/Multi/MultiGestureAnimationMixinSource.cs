using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;

namespace Silksprite.EmoteWizard.Sources.Impl.Multi
{
    public class MultiGestureAnimationMixinSource : MultiAnimationMixinSourceBase, IGestureAnimationMixinSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Gesture;
    }
}