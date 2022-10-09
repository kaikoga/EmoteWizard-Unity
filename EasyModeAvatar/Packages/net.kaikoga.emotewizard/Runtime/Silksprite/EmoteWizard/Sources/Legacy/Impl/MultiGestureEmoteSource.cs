using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    [DisallowMultipleComponent]
    public class MultiGestureEmoteSource : MultiEmoteSourceBase, IGestureEmoteSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Gesture;
    }
}