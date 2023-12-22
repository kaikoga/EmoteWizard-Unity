using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Utils;
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
            var emptyClip = EnsureAsset(environment, GeneratedPaths.GeneratedEmpty, environment.EmptyClip);
            environment.EmptyClip = emptyClip;
            return emptyClip;
        }

        public static void AddWizard<T>(this EmoteWizardEnvironment environment) where T : EmoteWizardBase
        {
            var wizard = environment.GetComponentInChildren<T>();
            if (!wizard) Undo.AddComponent<T>(environment.ContainerTransform.gameObject);
        }

        public static T FindOrCreateChildComponent<T>(this EmoteWizardEnvironment self, string path, Action<T> initializer = null) where T : Component
        {
            if (self.Root)
            {
                var child = self.Root.transform.Find(path);
                if (child && child.EnsureComponent<T>() is T c) return c;
            }
            if (self.AvatarDescriptor)
            {
                var child = self.AvatarDescriptor.transform.Find(path);
                if (child && child.EnsureComponent<T>() is T c) return c;
            }
            return self.ContainerTransform.AddChildComponent(path, initializer);
        }
    }
}