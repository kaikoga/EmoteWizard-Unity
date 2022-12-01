using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EmoteSequenceSource))]
    public class EmoteSequenceSourceEditor : Editor
    {
        SerializedProperty _serializedIsFixedDuration;

        SerializedProperty _serializedClip;
        SerializedProperty _serializedEntryTransitionDuration;
        SerializedProperty _serializedExitTransitionDuration;

        SerializedProperty _serializedHasExitTime;
        SerializedProperty _serializedClipExitTime;

        SerializedProperty _serializedHasTimeParameter;
        SerializedProperty _serializedTimeParameter;

        SerializedProperty _serializedHasEntryClip;
        SerializedProperty _serializedEntryClip;
        SerializedProperty _serializedEntryClipExitTime;
        SerializedProperty _serializedPostEntryTransitionDuration;

        SerializedProperty _serializedHasExitClip;
        SerializedProperty _serializedExitClip;
        SerializedProperty _serializedExitClipExitTime;
        SerializedProperty _serializedPostExitTransitionDuration;

        SerializedProperty _serializedHasLayerBlend;
        SerializedProperty _serializedBlendIn;
        SerializedProperty _serializedBlendOut;

        SerializedProperty _serializedHasTrackingOverrides;
        SerializedProperty _serializedTrackingOverrides;

        void OnEnable()
        {
            var serializedItem = serializedObject.FindProperty(nameof(EmoteSequenceSource.sequence));

            _serializedIsFixedDuration = serializedItem.FindPropertyRelative(nameof(EmoteSequence.isFixedDuration));
            
            _serializedClip = serializedItem.FindPropertyRelative(nameof(EmoteSequence.clip));
            _serializedEntryTransitionDuration = serializedItem.FindPropertyRelative(nameof(EmoteSequence.entryTransitionDuration));
            _serializedExitTransitionDuration = serializedItem.FindPropertyRelative(nameof(EmoteSequence.exitTransitionDuration));
            
            _serializedHasExitTime = serializedItem.FindPropertyRelative(nameof(EmoteSequence.hasExitTime));
            _serializedClipExitTime = serializedItem.FindPropertyRelative(nameof(EmoteSequence.clipExitTime));

            _serializedHasTimeParameter = serializedItem.FindPropertyRelative(nameof(EmoteSequence.hasTimeParameter));
            _serializedTimeParameter = serializedItem.FindPropertyRelative(nameof(EmoteSequence.timeParameter));

            _serializedHasEntryClip = serializedItem.FindPropertyRelative(nameof(EmoteSequence.hasEntryClip));
            _serializedEntryClip = serializedItem.FindPropertyRelative(nameof(EmoteSequence.entryClip));
            _serializedEntryClipExitTime = serializedItem.FindPropertyRelative(nameof(EmoteSequence.entryClipExitTime));
            _serializedPostEntryTransitionDuration = serializedItem.FindPropertyRelative(nameof(EmoteSequence.postEntryTransitionDuration));

            _serializedHasExitClip = serializedItem.FindPropertyRelative(nameof(EmoteSequence.hasExitClip));
            _serializedExitClip = serializedItem.FindPropertyRelative(nameof(EmoteSequence.exitClip));
            _serializedExitClipExitTime = serializedItem.FindPropertyRelative(nameof(EmoteSequence.exitClipExitTime));
            _serializedPostExitTransitionDuration = serializedItem.FindPropertyRelative(nameof(EmoteSequence.postExitTransitionDuration));

            _serializedHasLayerBlend = serializedItem.FindPropertyRelative(nameof(EmoteSequence.hasLayerBlend));
            _serializedBlendIn = serializedItem.FindPropertyRelative(nameof(EmoteSequence.blendIn));
            _serializedBlendOut = serializedItem.FindPropertyRelative(nameof(EmoteSequence.blendOut));

            _serializedHasTrackingOverrides = serializedItem.FindPropertyRelative(nameof(EmoteSequence.hasTrackingOverrides));
            _serializedTrackingOverrides = serializedItem.FindPropertyRelative(nameof(EmoteSequence.trackingOverrides));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedIsFixedDuration);
            EditorGUILayout.PropertyField(_serializedClip);
            EditorGUILayout.PropertyField(_serializedEntryTransitionDuration);
            EditorGUILayout.PropertyField(_serializedExitTransitionDuration);

            EditorGUILayout.PropertyField(_serializedHasExitTime);
            if (_serializedHasExitTime.boolValue)
            {
                EditorGUILayout.PropertyField(_serializedClipExitTime);
            }

            EditorGUILayout.PropertyField(_serializedHasTimeParameter);
            if (_serializedHasTimeParameter.boolValue)
            {
                EditorGUILayout.PropertyField(_serializedTimeParameter);
            }

            EditorGUILayout.PropertyField(_serializedHasEntryClip);
            if (_serializedHasEntryClip.boolValue)
            {
                EditorGUILayout.PropertyField(_serializedEntryClip);
                EditorGUILayout.PropertyField(_serializedEntryClipExitTime);
                EditorGUILayout.PropertyField(_serializedPostEntryTransitionDuration);
            }

            EditorGUILayout.PropertyField(_serializedHasExitClip);
            if (_serializedHasExitClip.boolValue)
            {
                EditorGUILayout.PropertyField(_serializedExitClip);
                EditorGUILayout.PropertyField(_serializedExitClipExitTime);
                EditorGUILayout.PropertyField(_serializedPostExitTransitionDuration);
            }

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
        }
    }
}
