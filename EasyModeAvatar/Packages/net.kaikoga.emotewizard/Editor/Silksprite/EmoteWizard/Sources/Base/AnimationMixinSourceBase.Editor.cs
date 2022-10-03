using System;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Impl.Base;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    [CustomEditor(typeof(AnimationMixinSourceBase), true)]
    public class AnimationMixinSourceBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var animationMixinSourceBase = (AnimationMixinSourceBase)target;
            var layer = animationMixinSourceBase.LayerName;
            var mixin = animationMixinSourceBase.mixin;

            var serializedObj = serializedObject.FindProperty(nameof(AnimationMixinSourceBase.mixin));

            using (new EditorGUI.DisabledScope(!mixin.enabled))
            {
                EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(AnimationMixin.name)));
                EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(AnimationMixin.kind)));
                
                switch (mixin.kind)
                {
                    case AnimationMixinKind.AnimationClip:
                        CustomEditorGUILayout.PropertyFieldWithGenerate(
                            serializedObj.FindPropertyRelative(nameof(AnimationMixin.animationClip)),
                            () =>
                            {
                                if (string.IsNullOrEmpty(name))
                                {
                                    Debug.LogError("Mixin Name is required.");
                                    return null;
                                }

                                var relativePath = $"{GeneratedAssetLocator.MixinDirectoryPath(layer)}@@@Generated@@@Mixin_{name}.anim";
                                return animationMixinSourceBase.EmoteWizardRoot.EnsureAsset<AnimationClip>(relativePath);
                            });
                        break;
                    case AnimationMixinKind.BlendTree:
                        CustomEditorGUILayout.PropertyFieldWithGenerate(
                            serializedObj.FindPropertyRelative(nameof(AnimationMixin.blendTree)),
                            () =>
                            {
                                if (string.IsNullOrEmpty(name))
                                {
                                    Debug.LogError("Mixin Name is required.");
                                    return null;
                                }

                                var relativePath = $"{GeneratedAssetLocator.MixinDirectoryPath(layer)}@@@Generated@@@Mixin_{name}.anim";
                                return animationMixinSourceBase.EmoteWizardRoot.EnsureAsset<BlendTree>(relativePath);
                            });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(AnimationMixinSourceBase.isBaseMixin)));

                EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(AnimationMixin.conditions)));
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}