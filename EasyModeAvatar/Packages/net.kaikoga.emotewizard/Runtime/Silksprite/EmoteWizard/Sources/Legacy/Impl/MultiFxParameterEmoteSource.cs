using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    public class MultiFxParameterEmoteSource : MultiParameterEmoteSourceBase, IFxParameterEmoteSource
    {
        public virtual string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}