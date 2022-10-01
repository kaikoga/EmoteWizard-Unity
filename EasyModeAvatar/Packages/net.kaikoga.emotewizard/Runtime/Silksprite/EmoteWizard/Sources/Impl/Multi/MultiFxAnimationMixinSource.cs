using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;

namespace Silksprite.EmoteWizard.Sources.Impl.Multi
{
    public class MultiFxAnimationMixinSource : MultiAnimationMixinSourceBase, IFxAnimationMixinSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}