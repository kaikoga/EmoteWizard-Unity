using UnityEditor;
using UnityEngine;

using static EmoteWizard.Tools.EmoteWizardEditorTools;

namespace EmoteWizard.Extensions
{
    public static partial class EmoteWizardRootExtension
    {
        public static AnimationClip ProvideEmptyClip(this EmoteWizardRoot root)
        {
            return EnsureAsset(root, "@@@Generated@@@Empty.anim", ref root.emptyClip);
        }
    }
}