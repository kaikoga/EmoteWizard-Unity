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
        SerializedProperty _serializedName;
        SerializedProperty _serializedKind;
        SerializedProperty _serializedAnimationClip;
        SerializedProperty _serializedBlendTree;
        SerializedProperty _serializedConditions;

        SerializedProperty _serializedIsBaseMixin;

        void OnEnable()
        {
            var serializedItem = serializedObject.FindProperty(nameof(AnimationMixinSourceBase.mixin));

            _serializedName = serializedItem.FindPropertyRelative(nameof(AnimationMixin.name));
            _serializedKind = serializedItem.FindPropertyRelative(nameof(AnimationMixin.kind));
            _serializedAnimationClip = serializedItem.FindPropertyRelative(nameof(AnimationMixin.animationClip));
            _serializedBlendTree = serializedItem.FindPropertyRelative(nameof(AnimationMixin.blendTree));
            _serializedConditions = serializedItem.FindPropertyRelative(nameof(AnimationMixin.conditions));

            _serializedIsBaseMixin = serializedObject.FindProperty(nameof(AnimationMixinSourceBase.isBaseMixin));
        }

        public override void OnInspectorGUI()
        {
            var animationMixinSourceBase = (AnimationMixinSourceBase)target;
            var layer = animationMixinSourceBase.LayerName;
            var mixin = animationMixinSourceBase.mixin;

            using (new EditorGUI.DisabledScope(!mixin.enabled))
            {
                EditorGUILayout.PropertyField(_serializedName);
                EditorGUILayout.PropertyField(_serializedKind);
                
                switch (mixin.kind)
                {
                    case AnimationMixinKind.AnimationClip:
                        CustomEditorGUILayout.PropertyFieldWithGenerate(
                            _serializedAnimationClip,
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
                            _serializedBlendTree,
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

                EditorGUILayout.PropertyField(_serializedConditions);
                EditorGUILayout.PropertyField(_serializedIsBaseMixin);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}