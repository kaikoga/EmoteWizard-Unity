using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl.Base;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class FxParameterEmoteSource : ParameterEmoteSourceBase, IFxParameterEmoteSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}