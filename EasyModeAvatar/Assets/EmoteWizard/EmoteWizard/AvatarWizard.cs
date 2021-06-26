using EmoteWizard.Base;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class AvatarWizard : EmoteWizardBase
    {
        [SerializeField] public VRCAvatarDescriptor avatarDescriptor;
        [SerializeField] public Animator proxyAnimator;

    }
}