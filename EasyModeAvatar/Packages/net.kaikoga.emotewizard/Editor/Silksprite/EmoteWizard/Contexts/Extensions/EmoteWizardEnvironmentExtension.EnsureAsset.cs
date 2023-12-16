using System.IO;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static partial class EmoteWizardEnvironmentExtension
    {
        public static T EnsureAsset<T>(this EmoteWizardEnvironment environment, GeneratedPath relativePath, T asset = null)
            where T : Object, new()
        {
            if (asset) return asset;
            var assetPath = relativePath.Resolve(environment);
            if (environment.PersistGeneratedAssets)
            {
                asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset) return asset;
            }
            return environment.CreateAsset<T>(assetPath);
        }

        static T CreateAsset<T>(this EmoteWizardEnvironment environment, string assetPath)
            where T : Object, new()
        {
            var asset = typeof(ScriptableObject).IsAssignableFrom(typeof(T)) ? ScriptableObject.CreateInstance(typeof(T)) as T : new T();
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