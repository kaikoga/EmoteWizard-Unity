using System;
using System.IO;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class EmoteWizardBaseExtension
    {
        static void DestroyAllSubAssets<T>(T outputAsset, Func<Object, bool> isSubAsset = null) where T : Object
        {
            if (!outputAsset.IsPersistedAsset()) return;
            isSubAsset = isSubAsset ?? AssetDatabase.IsSubAsset;
            AssetDatabase
                .LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(outputAsset))
                .Where(isSubAsset)
                .ToList()
                .ForEach(subAsset => Object.DestroyImmediate(subAsset, true));
        }

        public static T ReplaceOrCreateOutputAsset<T>(this EmoteWizardBase emoteWizardBase, ref T outputAsset, string defaultRelativePath)
            where T:ScriptableObject
        {
            if (emoteWizardBase.Context.PersistGeneratedAssets)
            {
                if (outputAsset && outputAsset.IsPersistedAsset())
                {
                    var o = outputAsset;
                    DestroyAllSubAssets(outputAsset, asset => asset != o);
                    var value = ScriptableObject.CreateInstance<T>();
                    EditorUtility.CopySerialized(value, outputAsset);
                }
                else
                {
                    outputAsset = ScriptableObject.CreateInstance<T>();
                    var path = emoteWizardBase.Context.GeneratedAssetPath(defaultRelativePath);
                    EnsureDirectory(path);
                    AssetDatabase.CreateAsset(outputAsset, path);
                }

                EditorUtility.SetDirty(outputAsset);
            }
            else
            {
                var path = emoteWizardBase.Context.GeneratedAssetPath(defaultRelativePath);
                outputAsset = ScriptableObject.CreateInstance<T>();
                outputAsset.name = Path.GetFileNameWithoutExtension(path);
            }

            EditorUtility.SetDirty(emoteWizardBase);
            return outputAsset;
        }

        public static AnimatorController ReplaceOrCreateOutputAsset(this EmoteWizardBase emoteWizardBase, ref RuntimeAnimatorController outputAsset, string defaultRelativePath)
        {
            AnimatorController animatorController;

            if (emoteWizardBase.Context.PersistGeneratedAssets)
            {
                animatorController = outputAsset as AnimatorController;
                if (animatorController)
                {
                    DestroyAllSubAssets(animatorController, asset => asset != animatorController);
                    animatorController.layers = new AnimatorControllerLayer[] { };
                    animatorController.parameters = new AnimatorControllerParameter[] { };
                }
                else
                {
                    var path = emoteWizardBase.Context.GeneratedAssetPath(defaultRelativePath);
                    EnsureDirectory(path);
                    animatorController = AnimatorController.CreateAnimatorControllerAtPath(path);
                    animatorController.RemoveLayer(0); // Remove Base Layer
                    outputAsset = animatorController;
                }

                EditorUtility.SetDirty(animatorController);
            }
            else
            {
                var path = emoteWizardBase.Context.GeneratedAssetPath(defaultRelativePath);
                animatorController = new AnimatorController
                {
                    name = Path.GetFileNameWithoutExtension(path)
                };
                outputAsset = animatorController;
            }

            EditorUtility.SetDirty(emoteWizardBase);
            return animatorController;
        }
    }
}