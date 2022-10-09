using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    public class FxParameterEmoteSource : ParameterEmoteSourceBase, IFxParameterEmoteSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}