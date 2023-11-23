using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class ActionLayerWizard : AnimatorLayerWizardBase
    {
        public override LayerKind LayerKind => LayerKind.Action;

        [SerializeField] public bool hasResetClip = false;
        public override bool HasResetClip => hasResetClip;

        public override AnimatorLayerContextBase GetContext() => new ActionLayerContext(this);
    }
}