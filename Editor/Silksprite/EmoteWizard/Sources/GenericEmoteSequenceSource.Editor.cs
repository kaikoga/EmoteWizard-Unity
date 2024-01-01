using System;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GenericEmoteSequenceSource))]
    public class SimpleEmoteSourceEditor : Editor
    {
        SerializedProperty _serializedLayerKind;
        SerializedProperty _serializedGroupName;

        SerializedProperty _serializedAnimatedEnable;
        SerializedProperty _serializedAnimatedBlendShapes;

        SerializedProperty _serializedIsFixedDuration;
        SerializedProperty _serializedEntryTransitionDuration;
        SerializedProperty _serializedExitTransitionDuration;

        SerializedProperty _serializedHasLayerBlend;
        SerializedProperty _serializedBlendIn;
        SerializedProperty _serializedBlendOut;

        SerializedProperty _serializedHasTrackingOverrides;
        SerializedProperty _serializedTrackingOverrides;

        GenericEmoteSequenceSource _genericEmoteSequenceSource;

        void OnEnable()
        {
            var serializedItem = serializedObject.FindProperty(nameof(GenericEmoteSequenceSource.sequence));

            _serializedLayerKind = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.layerKind));
            _serializedGroupName = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.groupName));

            _serializedAnimatedEnable = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.animatedEnable));
            _serializedAnimatedBlendShapes = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.animatedBlendShapes));

            _serializedIsFixedDuration = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.isFixedDuration));
            _serializedEntryTransitionDuration = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.entryTransitionDuration));
            _serializedExitTransitionDuration = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.exitTransitionDuration));
            
            _serializedHasLayerBlend = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.hasLayerBlend));
            _serializedBlendIn = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.blendIn));
            _serializedBlendOut = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.blendOut));

            _serializedHasTrackingOverrides = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.hasTrackingOverrides));
            _serializedTrackingOverrides = serializedItem.FindPropertyRelative(nameof(GenericEmoteSequence.trackingOverrides));

            _genericEmoteSequenceSource = (GenericEmoteSequenceSource)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedLayerKind);
            EditorGUILayout.PropertyField(_serializedGroupName);

            EditorGUILayout.PropertyField(_serializedAnimatedEnable);
            EditorGUILayout.PropertyField(_serializedAnimatedBlendShapes);

            EditorGUILayout.PropertyField(_serializedIsFixedDuration);
            EditorGUILayout.PropertyField(_serializedEntryTransitionDuration);
            EditorGUILayout.PropertyField(_serializedExitTransitionDuration);

            EditorGUILayout.PropertyField(_serializedHasLayerBlend);
            if (_serializedHasLayerBlend.boolValue)
            {
                EditorGUILayout.PropertyField(_serializedBlendIn);
                EditorGUILayout.PropertyField(_serializedBlendOut);
            }

            EditorGUILayout.PropertyField(_serializedHasTrackingOverrides);
            if (_serializedHasTrackingOverrides.boolValue)
            {
                EditorGUILayout.PropertyField(_serializedTrackingOverrides);
            }

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Explode"))
            {
                SourceExploder.ExplodeEmoteSequences(_genericEmoteSequenceSource);
                return;
            }

            _genericEmoteSequenceSource = (GenericEmoteSequenceSource)target;
            EmoteWizardGUILayout.Tutorial(_genericEmoteSequenceSource.CreateEnv(), Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "アニメーションの内容をここに登録します。",
                "Group Nameでアニメーションをグループ分けします。同時に再生したいアニメーションはGroupを分けてください。",
                "",
                "Layer Blend: VRC Playable Layer Controlの設定をします。現状Actionレイヤー専用です。",
                "Tracking Override: VRC Animator Tracking Controlの設定をします。アニメーションの再生中に一時的にAnimationにする項目を登録します。");

        [CustomPropertyDrawer(typeof(GenericEmoteSequence.AnimatedEnable))]
        public class AnimatedEnableDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
            {
                var serializedTarget = serializedProperty.FindPropertyRelative(nameof(GenericEmoteSequence.AnimatedEnable.target));
                var serializedIsEnable = serializedProperty.FindPropertyRelative(nameof(GenericEmoteSequence.AnimatedEnable.isEnable));

                EditorGUI.PropertyField(position.UISliceV(0), serializedTarget);
                EditorGUI.PropertyField(position.UISliceV(1), serializedIsEnable);
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 1;
            }
        }

        [CustomPropertyDrawer(typeof(GenericEmoteSequence.AnimatedBlendShape))]
        public class AnimatedBlendShapeDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
            {
                var serializedTarget = serializedProperty.FindPropertyRelative(nameof(GenericEmoteSequence.AnimatedBlendShape.target));
                var serializedBlendShapeName = serializedProperty.FindPropertyRelative(nameof(GenericEmoteSequence.AnimatedBlendShape.blendShapeName));
                var serializedValue = serializedProperty.FindPropertyRelative(nameof(GenericEmoteSequence.AnimatedBlendShape.value));

                EditorGUI.PropertyField(position.UISliceV(0), serializedTarget);

                var skinnedMeshRenderer = (SkinnedMeshRenderer)serializedTarget.objectReferenceValue;
                if (skinnedMeshRenderer && skinnedMeshRenderer.sharedMesh is Mesh sharedMesh)
                {
                    EditorGUI.BeginChangeCheck();
                    var options = Enumerable.Range(0, sharedMesh.blendShapeCount)
                        .Select(i => sharedMesh.GetBlendShapeName(i))
                        .ToArray();
                    var newBlendShapeNameValue = EditorGUI.Popup(
                        position.UISliceV(1),
                        serializedBlendShapeName.displayName,
                        Array.IndexOf(options, serializedBlendShapeName.stringValue),
                        options
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedBlendShapeName.stringValue = options[newBlendShapeNameValue];
                    }
                }

                EditorGUI.PropertyField(position.UISliceV(2), serializedValue);
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2;
            }
        }
    }
}
