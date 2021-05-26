using System.Linq;
using EmoteWizard.Base;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.EmoteWizardEditorTools;

namespace EmoteWizard.Extensions
{
    public static class EmoteWizardBaseExtension
    {
        public static T ReplaceOrCreateOutputAsset<T>(this EmoteWizardBase emoteWizardBase, ref T outputAsset, string defaultRelativePath)
            where T:ScriptableObject
        {
            if (outputAsset)
            {
                var assetPath = AssetDatabase.GetAssetPath(outputAsset);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    AssetDatabase
                        .LoadAllAssetsAtPath(assetPath)
                        .Where(AssetDatabase.IsSubAsset)
                        .ToList()
                        .ForEach(subAsset => Object.DestroyImmediate(subAsset, true));
                }

                var value = ScriptableObject.CreateInstance<T>();
                EditorUtility.CopySerialized(value, outputAsset);
            }
            else
            {
                var path = emoteWizardBase.EmoteWizardRoot.GeneratedAssetPath(defaultRelativePath);
                EnsureDirectory(path);
                outputAsset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(outputAsset, defaultRelativePath);
            }
            return outputAsset;
        }
    }
}