using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    public class GestureParameterEmoteSource : ParameterEmoteSourceBase, IGestureParameterEmoteSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Gesture;
    }
}