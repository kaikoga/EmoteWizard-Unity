using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [DisallowMultipleComponent]
    public class GestureEmoteSource : EmoteSourceBase
    {
        public override string LayerName => "Gesture";
    }
}