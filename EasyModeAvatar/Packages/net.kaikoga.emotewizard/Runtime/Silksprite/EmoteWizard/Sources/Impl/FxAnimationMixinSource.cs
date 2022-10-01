using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl.Base;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class FxAnimationMixinSource : AnimationMixinSourceBase, IFxAnimationMixinSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}