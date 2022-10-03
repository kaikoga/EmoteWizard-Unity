using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(ActionEmoteSource))]
    public class ActionEmoteSourceEditor : Editor
    {
        SerializedProperty _serializedHasExitTime;
        SerializedProperty _serializedBlendIn;
        SerializedProperty _serializedEntryTransitionDuration;
        SerializedProperty _serializedEntryClip;
        SerializedProperty _serializedEntryClipExitTime;
        SerializedProperty _serializedPostEntryTransitionDuration;
        SerializedProperty _serializedClip;
        SerializedProperty _serializedClipExitTime;
        SerializedProperty _serializedExitTransitionDuration;
        SerializedProperty _serializedExitClip;
        SerializedProperty _serializedExitClipExitTime;
        SerializedProperty _serializedPostExitTransitionDuration;
        SerializedProperty _serializedBlendOut;
        SerializedProperty _serializedName;
        SerializedProperty _serializedEmoteIndex;

        const bool IsDefaultAfk = false; // FIXME

        void OnEnable()
        {
            var serializedItem = serializedObject.FindProperty(nameof(ActionEmoteSource.actionEmote));

            _serializedName = serializedItem.FindPropertyRelative(nameof(ActionEmote.name));
            _serializedEmoteIndex = serializedItem.FindPropertyRelative(nameof(ActionEmote.emoteIndex));
            _serializedHasExitTime = serializedItem.FindPropertyRelative(nameof(ActionEmote.hasExitTime));
            _serializedBlendIn = serializedItem.FindPropertyRelative(nameof(ActionEmote.blendIn));
            _serializedEntryTransitionDuration = serializedItem.FindPropertyRelative(nameof(ActionEmote.entryTransitionDuration));
            _serializedEntryClip = serializedItem.FindPropertyRelative(nameof(ActionEmote.entryClip));
            _serializedEntryClipExitTime = serializedItem.FindPropertyRelative(nameof(ActionEmote.entryClipExitTime));
            _serializedPostEntryTransitionDuration = serializedItem.FindPropertyRelative(nameof(ActionEmote.postEntryTransitionDuration));
            _serializedClip = serializedItem.FindPropertyRelative(nameof(ActionEmote.clip));
            _serializedClipExitTime = serializedItem.FindPropertyRelative(nameof(ActionEmote.clipExitTime));
            _serializedExitTransitionDuration = serializedItem.FindPropertyRelative(nameof(ActionEmote.exitTransitionDuration));
            _serializedExitClip = serializedItem.FindPropertyRelative(nameof(ActionEmote.exitClip));
            _serializedExitClipExitTime = serializedItem.FindPropertyRelative(nameof(ActionEmote.exitClipExitTime));
            _serializedPostExitTransitionDuration = serializedItem.FindPropertyRelative(nameof(ActionEmote.postExitTransitionDuration));
            _serializedBlendOut = serializedItem.FindPropertyRelative(nameof(ActionEmote.blendOut));
        }

        public override void OnInspectorGUI()
        {
            var FixedTransitionDuration = ((ActionEmoteSource)target).EmoteWizardRoot.GetWizard<ActionWizard>().fixedTransitionDuration;
            
            EditorGUILayout.PropertyField(_serializedName);

            if (!IsDefaultAfk)
            {
                using (new InvalidValueScope(_serializedEmoteIndex.intValue == 0))
                {
                    EditorGUILayout.PropertyField(_serializedEmoteIndex);
                }
            }

            EditorGUILayout.PropertyField(_serializedHasExitTime);
            EditorGUILayout.PropertyField(_serializedBlendIn);

            TransitionField(FixedTransitionDuration, _serializedEntryTransitionDuration);
            ClipField(_serializedEntryClip, _serializedEntryClipExitTime);
            using (new EditorGUI.DisabledScope(_serializedEntryClip.objectReferenceValue == null))
            {
                TransitionField(FixedTransitionDuration, _serializedPostEntryTransitionDuration);
            }

            ClipField(_serializedClip, _serializedClipExitTime);

            TransitionField(FixedTransitionDuration, _serializedExitTransitionDuration);
            ClipField(_serializedExitClip, _serializedExitClipExitTime);
            using (new EditorGUI.DisabledScope(_serializedExitClip.objectReferenceValue == null))
            {
                TransitionField(FixedTransitionDuration, _serializedPostExitTransitionDuration);
            }

            EditorGUILayout.PropertyField(_serializedBlendOut);
            
            serializedObject.ApplyModifiedProperties();
        }
        
        static void TransitionField(bool fixedTransitionDuration, SerializedProperty serializedField)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(serializedField);
                if (fixedTransitionDuration)
                {
                    GUILayout.Label("(s)", GUILayout.Width(20f));
                }
            }
        }

        static void ClipField(SerializedProperty serializedClipField, SerializedProperty serializedExitTimeField)
        {
            EditorGUILayout.PropertyField(serializedClipField);
            var motion = (Motion)serializedClipField.objectReferenceValue;
            using (new EditorGUI.DisabledScope(motion == null))
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(serializedExitTimeField);
                if (motion == null) return;
                var realExitTime = serializedExitTimeField.floatValue * motion.averageDuration;
                GUILayout.Label($"{realExitTime:F2}s");
            }
        }
    }
}