using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;

namespace Silksprite.EmoteWizard.Sources.Impl.Multi
{
    public class MultiFxParameterEmoteSource : MultiParameterEmoteSourceBase, IFxParameterEmoteSource
    {
        public virtual string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}