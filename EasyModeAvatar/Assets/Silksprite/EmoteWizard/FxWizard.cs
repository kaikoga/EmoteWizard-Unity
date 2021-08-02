using Silksprite.EmoteWizard.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class FxWizard : AnimationWizardBase
    {
        [SerializeField] public AnimationClip resetClip;

        [SerializeField] public string handSignOverrideParameter = "EmoteWizardFX";

        public override string LayerName => "FX";
        public override string HandSignOverrideParameter => handSignOverrideParameter;
    }
}