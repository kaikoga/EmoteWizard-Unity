using System;
using System.IO;
using System.Linq;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Contexts.Extensions
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

        public static T ReplaceOrCreateOutputAsset<T>(this IOutputContext<T> context, string defaultRelativePath)
            where T : ScriptableObject
        {
            var outputAsset = context.OutputAsset;
            if (context.Environment.PersistGeneratedAssets)
            {
                if (outputAsset && outputAsset.IsPersistedAsset())
                {
                    DestroyAllSubAssets(outputAsset, asset => asset != outputAsset);
                    var value = ScriptableObject.CreateInstance<T>();
                    EditorUtility.CopySerialized(value, outputAsset);
                }
                else
                {
                    outputAsset = ScriptableObject.CreateInstance<T>();
                    var path = context.Environment.GeneratedAssetPath(defaultRelativePath);
                    EnsureDirectory(path);
                    AssetDatabase.CreateAsset(outputAsset, path);
                    context.OutputAsset = outputAsset;
                }

                EditorUtility.SetDirty(outputAsset);
            }
            else
            {
                var path = context.Environment.GeneratedAssetPath(defaultRelativePath);
                outputAsset = ScriptableObject.CreateInstance<T>();
                outputAsset.name = Path.GetFileNameWithoutExtension(path);
                context.OutputAsset = outputAsset;
            }

            if (context.GameObject)
            {
                EditorUtility.SetDirty(context.GameObject);
            }
            return outputAsset;
        }

        public static AnimatorController ReplaceOrCreateOutputAsset(this IOutputContext<RuntimeAnimatorController> context, string defaultRelativePath)
        {
            AnimatorController animatorController;

            if (context.Environment.PersistGeneratedAssets)
            {
                animatorController = context.OutputAsset as AnimatorController;
                if (animatorController)
                {
                    DestroyAllSubAssets(animatorController, asset => asset != animatorController);
                    animatorController.layers = new AnimatorControllerLayer[] { };
                    animatorController.parameters = new AnimatorControllerParameter[] { };
                }
                else
                {
                    var path = context.Environment.GeneratedAssetPath(defaultRelativePath);
                    EnsureDirectory(path);
                    animatorController = AnimatorController.CreateAnimatorControllerAtPath(path);
                    animatorController.RemoveLayer(0); // Remove Base Layer
                    context.OutputAsset = animatorController;
                }

                EditorUtility.SetDirty(animatorController);
            }
            else
            {
                var path = context.Environment.GeneratedAssetPath(defaultRelativePath);
                animatorController = new AnimatorController
                {
                    name = Path.GetFileNameWithoutExtension(path)
                };
                context.OutputAsset = animatorController;
            }

            if (context.GameObject)
            {
                EditorUtility.SetDirty(context.GameObject);
            }
            return animatorController;
        }

    }
}