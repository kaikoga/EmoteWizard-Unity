using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SimpleEmoteSource))]
    public class SimpleEmoteSourceEditor : Editor
    {
        SerializedProperty _serializedLayerKind;
        SerializedProperty _serializedGroupName;

        SerializedProperty _serializedAnimatedEnable;

        SerializedProperty _serializedIsFixedDuration;
        SerializedProperty _serializedEntryTransitionDuration;
        SerializedProperty _serializedExitTransitionDuration;

        SerializedProperty _serializedHasLayerBlend;
        SerializedProperty _serializedBlendIn;
        SerializedProperty _serializedBlendOut;

        SerializedProperty _serializedHasTrackingOverrides;
        SerializedProperty _serializedTrackingOverrides;

        SimpleEmoteSource _simpleEmoteSource;

        void OnEnable()
        {
            var serializedItem = serializedObject.FindProperty(nameof(SimpleEmoteSource.simpleEmote));

            _serializedLayerKind = serializedItem.FindPropertyRelative(nameof(SimpleEmote.layerKind));
            _serializedGroupName = serializedItem.FindPropertyRelative(nameof(SimpleEmote.groupName));

            _serializedAnimatedEnable = serializedItem.FindPropertyRelative(nameof(SimpleEmote.animatedEnable));

            _serializedIsFixedDuration = serializedItem.FindPropertyRelative(nameof(SimpleEmote.isFixedDuration));
            _serializedEntryTransitionDuration = serializedItem.FindPropertyRelative(nameof(SimpleEmote.entryTransitionDuration));
            _serializedExitTransitionDuration = serializedItem.FindPropertyRelative(nameof(SimpleEmote.exitTransitionDuration));
            
            _serializedHasLayerBlend = serializedItem.FindPropertyRelative(nameof(SimpleEmote.hasLayerBlend));
            _serializedBlendIn = serializedItem.FindPropertyRelative(nameof(SimpleEmote.blendIn));
            _serializedBlendOut = serializedItem.FindPropertyRelative(nameof(SimpleEmote.blendOut));

            _serializedHasTrackingOverrides = serializedItem.FindPropertyRelative(nameof(SimpleEmote.hasTrackingOverrides));
            _serializedTrackingOverrides = serializedItem.FindPropertyRelative(nameof(SimpleEmote.trackingOverrides));

            _simpleEmoteSource = (SimpleEmoteSource)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedLayerKind);
            EditorGUILayout.PropertyField(_serializedGroupName);

            EditorGUILayout.PropertyField(_serializedAnimatedEnable);

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
                SourceExploder.ExplodeEmoteSequences(_simpleEmoteSource);
                return;
            }

            _simpleEmoteSource = (SimpleEmoteSource)target;
            EmoteWizardGUILayout.Tutorial(_simpleEmoteSource.CreateEnv(), Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "アニメーションの内容をここに登録します。",
                "Group Nameでアニメーションをグループ分けします。同時に再生したいアニメーションはGroupを分けてください。",
                "",
                "Layer Blend: VRC Playable Layer Controlの設定をします。現状Actionレイヤー専用です。",
                "Tracking Override: VRC Animator Tracking Controlの設定をします。アニメーションの再生中に一時的にAnimationにする項目を登録します。");
    }
}
