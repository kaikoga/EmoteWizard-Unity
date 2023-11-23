using System;
using Silksprite.EmoteWizard.Base;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static partial class EmoteWizardEnvironmentExtension
    {
        public static AnimationClip ProvideEmptyClip(this EmoteWizardEnvironment environment)
        {
            var emptyClip = environment.EmptyClip;
            var asset = EnsureAsset(environment, "@@@Generated@@@Empty.anim", ref emptyClip);
            environment.EmptyClip = emptyClip;
            return asset;
        }

        public static T EnsureWizard<T>(this EmoteWizardEnvironment environment, Action<T> initializer = null) where T : EmoteWizardBase
        {
            var wizard = environment.GetComponentInChildren<T>();
            if (wizard) return wizard;

            wizard = Undo.AddComponent<T>(environment.GameObject);
            initializer?.Invoke(wizard);
            return wizard;
        }
    }
}