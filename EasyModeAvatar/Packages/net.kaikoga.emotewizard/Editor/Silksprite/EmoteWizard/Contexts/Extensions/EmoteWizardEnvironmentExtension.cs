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

        public static void AddWizard<T>(this EmoteWizardEnvironment environment) where T : EmoteWizardBase
        {
            var wizard = environment.GetComponentInChildren<T>();
            if (!wizard) Undo.AddComponent<T>(environment.GameObject);
        }
    }
}