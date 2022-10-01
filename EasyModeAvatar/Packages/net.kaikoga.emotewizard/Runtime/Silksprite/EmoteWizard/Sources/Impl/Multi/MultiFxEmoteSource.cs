using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl.Multi
{
    [DisallowMultipleComponent]
    public class MultiFxEmoteSource : MultiEmoteSourceBase, IFxEmoteSource
    {
        public override string LayerName => EmoteWizardConstants.LayerNames.Fx;
    }
}