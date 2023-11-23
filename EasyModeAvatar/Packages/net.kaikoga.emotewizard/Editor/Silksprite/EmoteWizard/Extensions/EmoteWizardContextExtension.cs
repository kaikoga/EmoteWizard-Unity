using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static partial class EmoteWizardContextExtension
    {
        public static AnimationClip ProvideEmptyClip(this IEmoteWizardEnvironment environment)
        {
            var emptyClip = environment.EmptyClip;
            var asset = EnsureAsset(environment, "@@@Generated@@@Empty.anim", ref emptyClip);
            environment.EmptyClip = emptyClip;
            return asset;
        }

        public static T EnsureWizard<T>(this IEmoteWizardEnvironment environment, Action<T> initializer = null) where T : EmoteWizardBase
        {
            var wizard = environment.GetComponentInChildren<T>();
            if (wizard) return wizard;

            wizard = Undo.AddComponent<T>(environment.GameObject);
            initializer?.Invoke(wizard);
            return wizard;
        }
    }
}