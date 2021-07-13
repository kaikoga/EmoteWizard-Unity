using Silksprite.EmoteWizard.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class FxWizard : AnimationWizardBase
    {
        [SerializeField] public AnimationClip resetClip;

        public override string LayerName => "FX";
    }
}