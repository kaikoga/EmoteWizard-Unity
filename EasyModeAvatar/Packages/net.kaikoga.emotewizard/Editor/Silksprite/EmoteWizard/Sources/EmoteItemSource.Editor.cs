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
        SerializedProperty _serializedLayerKind;
        SerializedProperty _serializedGroupName;
        SerializedProperty _serializedConditions;

        EmoteItemSource _emoteItemSource;

        void OnEnable()
        {
            var serializedTrigger = serializedObject.FindProperty(nameof(EmoteItemSource.trigger));

            _serializedName = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.name));
            _serializedLayerKind = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.layerKind));
            _serializedGroupName = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.groupName));
            _serializedConditions = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.conditions));

            _emoteItemSource = (EmoteItemSource)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedName);
            EditorGUILayout.PropertyField(_serializedLayerKind);
            EditorGUILayout.PropertyField(_serializedGroupName);
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
