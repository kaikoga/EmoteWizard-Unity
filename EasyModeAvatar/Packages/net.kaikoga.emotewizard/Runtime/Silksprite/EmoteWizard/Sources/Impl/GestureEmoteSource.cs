using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [DisallowMultipleComponent]
    public class GestureEmoteSource : EmoteSourceBase, IGestureEmoteSource
    {
        public override string LayerName => "Gesture";
    }
}