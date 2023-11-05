using Silksprite.EmoteWizard.Base;
using UnityEditor;
using UnityEngine;

using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard.Extensions
{
    public static partial class EmoteWizardContextExtension
    {
        public static T EnsureAsset<T>(this IEmoteWizardContext context, string relativePath)
            where T : Object, new()
        {
            T asset = default;
            return EnsureAsset(context, relativePath, ref asset);
        }

        public static T EnsureAsset<T>(this IEmoteWizardContext context, string relativePath, ref T asset)
            where T : Object, new()
        {
            if (asset) return asset;
            var assetPath = context.GeneratedAssetPath(relativePath);
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