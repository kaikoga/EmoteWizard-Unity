using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    [DisallowMultipleComponent]
    public class FxEmoteSource : EmoteSourceBase, IFxEmoteSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}