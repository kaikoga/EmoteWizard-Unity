using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEditor;

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
        SerializedProperty _serializedMirror;

        void OnEnable()
        {
            var serializedTrigger = serializedObject.FindProperty(nameof(EmoteItemSource.trigger));

            _serializedName = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.name));
            _serializedLayerKind = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.layerKind));
            _serializedGroupName = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.groupName));
            _serializedConditions = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.conditions));

            _serializedMirror = serializedObject.FindProperty(nameof(EmoteItemSource.mirror));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedName);
            EditorGUILayout.PropertyField(_serializedLayerKind);
            EditorGUILayout.PropertyField(_serializedGroupName);
            EditorGUILayout.PropertyField(_serializedConditions);
            EditorGUILayout.PropertyField(_serializedMirror);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
