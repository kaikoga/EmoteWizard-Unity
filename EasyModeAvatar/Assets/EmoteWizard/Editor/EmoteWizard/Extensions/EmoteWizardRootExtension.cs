using UnityEditor;
using UnityEngine;

using static EmoteWizard.Tools.EmoteWizardEditorTools;

namespace EmoteWizard.Extensions
{
    public static class EmoteWizardRootExtension
    {
        public static AnimationClip ProvideEmptyClip(this EmoteWizardRoot root)
        {
            return EnsureAsset(root, "@@@Generated@@@Empty.anim", ref root.emptyClip);
        }

        public static T EnsureAsset<T>(this EmoteWizardRoot root, string relativePath)
            where T : Object, new()
        {
            T asset = default;
            return EnsureAsset(root, relativePath, ref asset);
        }

        public static T EnsureAsset<T>(this EmoteWizardRoot root, string relativePath, ref T asset)
            where T : Object, new()
        {
            if (asset) return asset;
            var assetPath = root.GeneratedAssetPath(relativePath);
            var existingAsset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            asset = existingAsset ? existingAsset : CreateAsset<T>(assetPath);
            return asset;
        }

        static T CreateAsset<T>(string assetPath)
            where T : Object, new()
        {
            var asset = new T();
            EnsureDirectory(assetPath);
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            return asset;
        }
    }
}