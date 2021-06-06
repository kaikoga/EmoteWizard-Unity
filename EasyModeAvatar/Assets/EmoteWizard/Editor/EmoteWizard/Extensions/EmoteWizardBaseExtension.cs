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

        public static AnimatorController ReplaceOrCreateOutputAsset(this EmoteWizardBase emoteWizardBase, ref RuntimeAnimatorController outputAsset, string defaultRelativePath)
        {
            var animatorController = outputAsset as AnimatorController;
            if (animatorController)
            {
                DestroyAllSubAssets(animatorController);
                animatorController.layers = new AnimatorControllerLayer[] { };
                animatorController.parameters = new AnimatorControllerParameter[] { };
            }
            else
            {
                var path = emoteWizardBase.EmoteWizardRoot.GeneratedAssetPath(defaultRelativePath);
                EnsureDirectory(path);
                animatorController = AnimatorController.CreateAnimatorControllerAtPath(path);
                animatorController.RemoveLayer(0); // Remove Base Layer
            }
            animatorController.AddParameter("GestureLeft", AnimatorControllerParameterType.Int);
            animatorController.AddParameter("GestureLeftWeight", AnimatorControllerParameterType.Float);
            animatorController.AddParameter("GestureRight", AnimatorControllerParameterType.Int);
            animatorController.AddParameter("GestureRightWeight", AnimatorControllerParameterType.Float);
            EditorUtility.SetDirty(emoteWizardBase);
            EditorUtility.SetDirty(animatorController);
            return animatorController;
        }
    }
}