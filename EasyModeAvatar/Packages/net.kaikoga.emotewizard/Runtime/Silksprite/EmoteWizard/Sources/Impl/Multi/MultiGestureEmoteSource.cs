using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl.Multi
{
    [DisallowMultipleComponent]
    public class MultiGestureEmoteSource : MultiEmoteSourceBase, IGestureEmoteSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Gesture;
    }
}