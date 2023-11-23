using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class GestureLayerWizard : AnimatorLayerWizardBase
    {
        public override LayerKind LayerKind => LayerKind.Gesture;

        [SerializeField] public bool hasResetClip = false;
        public override bool HasResetClip => hasResetClip;
        
        public override AnimatorLayerContextBase GetContext() => new GestureLayerContext(this);
    }
}