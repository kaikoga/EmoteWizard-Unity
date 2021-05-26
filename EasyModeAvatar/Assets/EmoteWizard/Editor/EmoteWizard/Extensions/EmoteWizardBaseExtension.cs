using System.Linq;
using EmoteWizard.Base;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static EmoteWizard.Tools.EmoteWizardEditorTools;

namespace EmoteWizard.Extensions
{
    public static class EmoteWizardBaseExtension
    {
        static void DestroyAllSubAssets<T>(T outputAsset) where T : Object
        {
            var assetPath = AssetDatabase.GetAssetPath(outputAsset);
            if (string.IsNullOrEmpty(assetPath)) return; // not an asset
            AssetDatabase
                .LoadAllAssetsAtPath(assetPath)
                .Where(AssetDatabase.IsSubAsset)
                .ToList()
                .ForEach(subAsset => Object.DestroyImmediate(subAsset, true));
        }

        public static T ReplaceOrCreateOutputAsset<T>(this EmoteWizardBase emoteWizardBase, ref T outputAsset, string defaultRelativePath)
            where T:ScriptableObject
        {
            if (outputAsset)
            {
                DestroyAllSubAssets(outputAsset);
                var value = ScriptableObject.CreateInstance<T>();
                EditorUtility.CopySerialized(value, outputAsset);
            }
            else
            {
                var path = emoteWizardBase.EmoteWizardRoot.GeneratedAssetPath(defaultRelativePath);
                EnsureDirectory(path);
                outputAsset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(outputAsset, path);
            }
            EditorUtility.SetDirty(emoteWizardBase);
            EditorUtility.SetDirty(outputAsset);
            return outputAsset;
        }

        public static AnimatorController ReplaceOrCreateOutputAsset(this EmoteWizardBase emoteWizardBase, ref AnimatorController outputAsset, string defaultRelativePath)
        {
            if (outputAsset)
            {
                DestroyAllSubAssets(outputAsset);
                outputAsset.layers = new AnimatorControllerLayer[] { };
                outputAsset.parameters = new AnimatorControllerParameter[] { };
            }
            else
            {
                var path = emoteWizardBase.EmoteWizardRoot.GeneratedAssetPath(defaultRelativePath);
                EnsureDirectory(path);
                outputAsset = AnimatorController.CreateAnimatorControllerAtPath(path);
                outputAsset.RemoveLayer(0); // Remove Base Layer
            }
            outputAsset.AddParameter("GestureLeft", AnimatorControllerParameterType.Int);
            outputAsset.AddParameter("GestureLeftWeight", AnimatorControllerParameterType.Float);
            outputAsset.AddParameter("GestureRight", AnimatorControllerParameterType.Int);
            outputAsset.AddParameter("GestureRightWeight", AnimatorControllerParameterType.Float);
            EditorUtility.SetDirty(emoteWizardBase);
            EditorUtility.SetDirty(outputAsset);
            return outputAsset;
        }
    }
}