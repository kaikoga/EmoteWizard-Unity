using System.IO;
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
            asset = existingAsset ? existingAsset : context.CreateAsset<T>(assetPath);
            return asset;
        }

        static T CreateAsset<T>(this IEmoteWizardContext context, string assetPath)
            where T : Object, new()
        {
            var asset = new T();
            if (context.PersistGeneratedAssets) 
            {
                EnsureDirectory(assetPath);
                AssetDatabase.CreateAsset(asset, assetPath);
                AssetDatabase.SaveAssets();
            }
            else
            {
                asset.name = Path.GetFileName(assetPath);
            }
            return asset;
        }
    }
}