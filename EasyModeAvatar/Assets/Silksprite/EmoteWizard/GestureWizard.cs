using Silksprite.EmoteWizard.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class GestureWizard : AnimationWizardBase
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        public override string LayerName => "Gesture";
    }
}