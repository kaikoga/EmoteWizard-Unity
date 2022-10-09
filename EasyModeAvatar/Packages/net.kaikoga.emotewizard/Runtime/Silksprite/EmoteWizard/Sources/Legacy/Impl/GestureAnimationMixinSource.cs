using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    public class GestureAnimationMixinSource : AnimationMixinSourceBase, IGestureAnimationMixinSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Gesture;
    }
}