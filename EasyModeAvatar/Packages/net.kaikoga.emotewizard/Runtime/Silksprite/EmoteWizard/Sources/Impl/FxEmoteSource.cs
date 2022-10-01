using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [DisallowMultipleComponent]
    public class FxEmoteSource : EmoteSourceBase, IFxEmoteSource
    {
        public override string LayerName => "FX";
    }
}