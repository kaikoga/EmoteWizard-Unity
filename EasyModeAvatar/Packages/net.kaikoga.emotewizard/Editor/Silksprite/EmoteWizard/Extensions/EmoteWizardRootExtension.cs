using Silksprite.EmoteWizard.Base;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static partial class EmoteWizardRootExtension
    {
        public static AnimationClip ProvideEmptyClip(this EmoteWizardRoot root)
        {
            return EnsureAsset(root, "@@@Generated@@@Empty.anim", ref root.emptyClip);
        }

        public static T EnsureWizard<T>(this EmoteWizardRoot root) where T : EmoteWizardBase
        {
            var wizard = root.GetComponentInChildren<T>();
            return wizard ? wizard : Undo.AddComponent<T>(root.gameObject);
        }
    }
}