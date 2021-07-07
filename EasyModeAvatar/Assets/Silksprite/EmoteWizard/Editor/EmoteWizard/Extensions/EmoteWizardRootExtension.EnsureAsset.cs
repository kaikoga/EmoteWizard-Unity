using UnityEditor;
using UnityEngine;

using static Silksprite.EmoteWizard.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard.Extensions
{
    public static partial class EmoteWizardRootExtension
    {
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