using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    public class FxAnimationMixinSource : AnimationMixinSourceBase, IFxAnimationMixinSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}