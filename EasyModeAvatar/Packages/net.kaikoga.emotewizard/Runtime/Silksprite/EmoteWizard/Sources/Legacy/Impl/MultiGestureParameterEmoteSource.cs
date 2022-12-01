using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    public class MultiGestureParameterEmoteSource : MultiParameterEmoteSourceBase, IGestureParameterEmoteSource
    {
        public virtual string LayerName => EmoteWizardConstants.LayerNames.Gesture;
    }
}