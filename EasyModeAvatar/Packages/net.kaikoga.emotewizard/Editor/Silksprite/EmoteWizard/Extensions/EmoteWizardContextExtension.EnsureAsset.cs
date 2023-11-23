using System.IO;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEditor;
using UnityEngine;

using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard.Extensions
{
    public static partial class EmoteWizardContextExtension
    {
        public static T EnsureAsset<T>(this EmoteWizardEnvironment environment, string relativePath)
            where T : Object, new()
        {
            T asset = default;
            return EnsureAsset(environment, relativePath, ref asset);
        }

        public static T EnsureAsset<T>(this EmoteWizardEnvironment environment, string relativePath, ref T asset)
            where T : Object, new()
        {
            if (asset) return asset;
            var assetPath = environment.GeneratedAssetPath(relativePath);
            var existingAsset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            asset = existingAsset ? existingAsset : environment.CreateAsset<T>(assetPath);
            return asset;
        }

        static T CreateAsset<T>(this EmoteWizardEnvironment environment, string assetPath)
            where T : Object, new()
        {
            var asset = new T();
            if (environment.PersistGeneratedAssets) 
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