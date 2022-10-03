using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(AfkEmoteSource))]
    public class AfkEmoteSourceEditor : Editor
    {
        const bool IsDefaultAfk = false; // FIXME

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject.FindProperty(nameof(AfkEmoteSource.afkEmote));
            var FixedTransitionDuration = ((AfkEmoteSource)target).EmoteWizardRoot.GetWizard<ActionWizard>().fixedTransitionDuration;

            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ActionEmote.name)));

            if (!IsDefaultAfk)
            {
                using (new InvalidValueScope(serializedObj.FindPropertyRelative(nameof(ActionEmote.emoteIndex)).intValue == 0))
                {
                    EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ActionEmote.emoteIndex)));
                }
            }

            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ActionEmote.hasExitTime)));
            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ActionEmote.blendIn)));

            TransitionField(FixedTransitionDuration, serializedObj.FindPropertyRelative(nameof(ActionEmote.entryTransitionDuration)));
            ClipField(serializedObj.FindPropertyRelative(nameof(ActionEmote.entryClip)),
                serializedObj.FindPropertyRelative(nameof(ActionEmote.entryClipExitTime)));
            using (new EditorGUI.DisabledScope(serializedObj.FindPropertyRelative(nameof(ActionEmote.entryClip)).objectReferenceValue == null))
            {
                TransitionField(FixedTransitionDuration, serializedObj.FindPropertyRelative(nameof(ActionEmote.postEntryTransitionDuration)));
            }

            ClipField(serializedObj.FindPropertyRelative(nameof(ActionEmote.clip)),
                serializedObj.FindPropertyRelative(nameof(ActionEmote.clipExitTime)));

            TransitionField(FixedTransitionDuration, serializedObj.FindPropertyRelative(nameof(ActionEmote.exitTransitionDuration)));
            ClipField(serializedObj.FindPropertyRelative(nameof(ActionEmote.exitClip)),
                serializedObj.FindPropertyRelative(nameof(ActionEmote.exitClipExitTime)));
            using (new EditorGUI.DisabledScope(serializedObj.FindPropertyRelative(nameof(ActionEmote.exitClip)).objectReferenceValue == null))
            {
                TransitionField(FixedTransitionDuration, serializedObj.FindPropertyRelative(nameof(ActionEmote.postExitTransitionDuration)));
            }

            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ActionEmote.blendOut)));
            
            serializedObject.ApplyModifiedProperties();
        }
        
        static void TransitionField(bool fixedTransitionDuration, SerializedProperty propertyField)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(propertyField);
                if (fixedTransitionDuration)
                {
                    GUILayout.Label("(s)", GUILayout.Width(20f));
                }
            }
        }

        static void ClipField(SerializedProperty propertyClipField, SerializedProperty propertyExitTimeField)
        {
            EditorGUILayout.PropertyField(propertyClipField);
            var motion = (Motion)propertyClipField.objectReferenceValue;
            using (new EditorGUI.DisabledScope(motion == null))
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(propertyExitTimeField);
                if (motion == null) return;
                var realExitTime = propertyExitTimeField.floatValue * motion.averageDuration;
                GUILayout.Label($"{realExitTime:F2}s");
            }
        }
    }
}