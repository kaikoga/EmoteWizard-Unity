using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class FxLayerWizard : AnimatorLayerWizardBase
    {
        public override LayerKind LayerKind => LayerKind.FX;

        [SerializeField] public bool hasResetClip = true;
        public override bool HasResetClip => hasResetClip;

        public override AnimatorLayerContextBase GetContext() => new FxLayerContext(this);
    }
}