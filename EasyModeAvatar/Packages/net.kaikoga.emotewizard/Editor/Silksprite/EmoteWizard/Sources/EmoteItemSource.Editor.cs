using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EmoteItemSource))]
    public class EmoteItemSourceEditor : Editor
    {
        SerializedProperty _serializedName;
        SerializedProperty _serializedPriority;
        SerializedProperty _serializedConditions;

        EmoteItemSource _emoteItemSource;

        void OnEnable()
        {
            var serializedTrigger = serializedObject.FindProperty(nameof(EmoteItemSource.trigger));

            _serializedName = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.name));
            _serializedPriority = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.priority));
            _serializedConditions = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.conditions));

            _emoteItemSource = (EmoteItemSource)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedName);
            EditorGUILayout.PropertyField(_serializedPriority);
            EditorGUILayout.PropertyField(_serializedConditions);
            serializedObject.ApplyModifiedProperties();

            if (_emoteItemSource.IsMirrorItem)
            {
                EditorGUILayout.HelpBox(MirrorInfoText, MessageType.Info);
            }
        }
        
        static string MirrorInfoText =>
            string.Join("\n",
                "パラメータのミラーリングが有効になっています。",
                "Gesture: GestureLeft / GestureRight",
                "GestureOther: GestureRight / GestureLeft",
                "GestureWeight: GestureLeftWeight / GestureRightWeight",
                "GestureOtherWeight: GestureRightWeight / GestureLeftWeight");
    }
}
