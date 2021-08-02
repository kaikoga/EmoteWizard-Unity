using Silksprite.EmoteWizard.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class GestureWizard : AnimationWizardBase
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public string handSignOverrideParameter = "EmoteWizardGesture";

        public override string LayerName => "Gesture";
        public override string HandSignOverrideParameter => handSignOverrideParameter;
    }
}