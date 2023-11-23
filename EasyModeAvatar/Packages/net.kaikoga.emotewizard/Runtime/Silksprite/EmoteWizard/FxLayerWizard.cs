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

        public override IAnimatorLayerWizardContext GetContext() => new FxLayerContext(this);

        class FxLayerContext : AnimatorLayerContextBase, IFxLayerWizardContext
        {
            public FxLayerContext(AnimatorLayerWizardBase wizard) : base(wizard) { }
        }
    }

    public interface IFxLayerWizardContext : IAnimatorLayerWizardContext { }
}