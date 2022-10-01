using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl.Base;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class GestureParameterEmoteSource : ParameterEmoteSourceBase, IGestureParameterEmoteSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Gesture;
    }
}