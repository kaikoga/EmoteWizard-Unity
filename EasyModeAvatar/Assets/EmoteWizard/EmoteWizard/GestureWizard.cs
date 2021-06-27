using EmoteWizard.Base;
using UnityEngine;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class GestureWizard : AnimationWizardBase
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public Motion globalClip;
        [SerializeField] public Motion ambienceClip;
    }
}