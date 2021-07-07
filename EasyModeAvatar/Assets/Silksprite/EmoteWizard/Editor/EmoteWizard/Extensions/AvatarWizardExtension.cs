using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class AvatarWizardExtension
    {
        public static Animator ProvideProxyAnimator(this AvatarWizard avatarWizard)
        {
            var animator = avatarWizard.proxyAnimator ? avatarWizard.proxyAnimator : avatarWizard.avatarDescriptor.EnsureComponent<Animator>();
            avatarWizard.proxyAnimator = animator;
            return animator;
        }
    }
}