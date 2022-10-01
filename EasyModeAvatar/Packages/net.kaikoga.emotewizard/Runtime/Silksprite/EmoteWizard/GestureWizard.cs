using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class GestureWizard : AnimationWizardBase<GestureEmoteSource, GestureParameterEmoteSource, GestureAnimationMixinSource>
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public string handSignOverrideParameter = "EmoteWizardGesture";

        public override string LayerName => "Gesture";
        public override string HandSignOverrideParameter => handSignOverrideParameter;
    }
}