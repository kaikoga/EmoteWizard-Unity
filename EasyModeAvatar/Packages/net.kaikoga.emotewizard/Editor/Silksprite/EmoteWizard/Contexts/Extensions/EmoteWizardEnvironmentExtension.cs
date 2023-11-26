using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static partial class EmoteWizardEnvironmentExtension
    {
        public static Animator ProvideProxyAnimator(this EmoteWizardEnvironment environment)
        {
            var animator = environment.ProxyAnimator ? environment.ProxyAnimator : environment.AvatarDescriptor.EnsureComponent<Animator>();
            environment.ProxyAnimator = animator;
            return animator;
        }

        public static AnimationClip ProvideEmptyClip(this EmoteWizardEnvironment environment)
        {
            var emptyClip = EnsureAsset(environment, "@@@Generated@@@Empty.anim", environment.EmptyClip);
            environment.EmptyClip = emptyClip;
            return emptyClip;
        }

        public static void AddWizard<T>(this EmoteWizardEnvironment environment) where T : EmoteWizardBase
        {
            var wizard = environment.GetComponentInChildren<T>();
            if (!wizard) Undo.AddComponent<T>(environment.ContainerTransform.gameObject);
        }
    }
}