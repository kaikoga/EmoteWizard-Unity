using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class FxAnimationMixinSource : MultiAnimationMixinSourceBase, IFxAnimationMixinSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}