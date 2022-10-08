using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;

namespace Silksprite.EmoteWizard.Sources.Impl.Multi
{
    public class MultiGestureParameterEmoteSource : MultiParameterEmoteSourceBase, IGestureParameterEmoteSource
    {
        public virtual string LayerName => EmoteWizardConstants.LayerNames.Gesture;
    }
}