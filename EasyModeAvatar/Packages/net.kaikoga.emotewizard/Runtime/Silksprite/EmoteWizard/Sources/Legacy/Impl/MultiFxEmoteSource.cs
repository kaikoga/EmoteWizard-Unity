using Silksprite.EmoteWizard.Sources.Legacy.Impl.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Legacy.Impl
{
    [DisallowMultipleComponent]
    public class MultiFxEmoteSource : MultiEmoteSourceBase, IFxEmoteSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}