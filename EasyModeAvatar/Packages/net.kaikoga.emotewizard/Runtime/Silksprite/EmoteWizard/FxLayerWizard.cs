using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class FxLayerWizard : AnimatorLayerWizardBase
    {
        public override LayerKind LayerKind => LayerKind.FX;
    }
}