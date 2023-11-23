using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static partial class EmoteWizardContextExtension
    {
        public static AnimationClip ProvideEmptyClip(this IEmoteWizardContext context)
        {
            var emptyClip = context.EmptyClip;
            var asset = EnsureAsset(context, "@@@Generated@@@Empty.anim", ref emptyClip);
            context.EmptyClip = emptyClip;
            return asset;
        }

        public static T EnsureWizard<T>(this IEmoteWizardContext context, Action<T> initializer = null) where T : EmoteWizardBase
        {
            var wizard = context.GetComponentInChildren<T>();
            if (wizard) return wizard;

            wizard = Undo.AddComponent<T>(context.GameObject);
            initializer?.Invoke(wizard);
            return wizard;
        }
    }
}