using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    [DisallowMultipleComponent]
    public class GestureEmoteSource : EmoteSourceBase, IGestureEmoteSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Gesture;
    }
}