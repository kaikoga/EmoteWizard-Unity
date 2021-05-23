using UnityEditor;
using UnityEngine;

using static EmoteWizard.Tools.EmoteWizardEditorTools;

namespace EmoteWizard.Extensions
{
    public static class EmoteWizardRootExtension
    {
        public static AnimationClip ProvideEmptyClip(this EmoteWizardRoot root)
        {
            return EnsureAnimationClip(root, "GeneratedEmpty.anim", ref root.emptyClip);
        }

        public static AnimationClip ProvideAnimationClip(this EmoteWizardRoot root, string relativePath)
        {
            var path = root.GeneratedAssetPath(relativePath);
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            return clip ? clip : EnsureAnimationClip(root, relativePath, ref clip);
        }

        public static AnimationClip EnsureAnimationClip(this EmoteWizardRoot root, string relativePath, ref AnimationClip clip)
        {
            if (clip) return clip;
            clip = root.CreateAnimationClip(relativePath);
            return clip;
        }

       static AnimationClip CreateAnimationClip(this EmoteWizardRoot root, string relativePath)
        {
            var path = root.GeneratedAssetPath(relativePath);
            var clip = new AnimationClip();
            EnsureDirectory(path);
            AssetDatabase.CreateAsset(clip, path);
            AssetDatabase.SaveAssets();
            return clip;
        }
    }
}