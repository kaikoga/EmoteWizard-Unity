using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(ParameterSource))]
    public class ParameterSourceEditor : Editor
    {
        SerializedProperty _serializedName;
        SerializedProperty _serializedItemKind;
        SerializedProperty _serializedDefaultValue;
        SerializedProperty _serializedSaved;
        SerializedProperty _serializedUsages;

        void OnEnable()
        {
            var serializedItem = serializedObject.FindProperty(nameof(ParameterSource.parameterItem));

            _serializedName = serializedItem.FindPropertyRelative(nameof(ParameterItem.name));
            _serializedItemKind = serializedItem.FindPropertyRelative(nameof(ParameterItem.itemKind));
            _serializedDefaultValue = serializedItem.FindPropertyRelative(nameof(ParameterItem.defaultValue));
            _serializedSaved = serializedItem.FindPropertyRelative(nameof(ParameterItem.saved));
            _serializedUsages = serializedItem.FindPropertyRelative(nameof(ParameterItem.usages));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedName);
            EditorGUILayout.PropertyField(_serializedItemKind);
            EditorGUILayout.PropertyField(_serializedDefaultValue);
            EditorGUILayout.PropertyField(_serializedSaved);

            EditorGUILayout.PropertyField(_serializedUsages);

            serializedObject.ApplyModifiedProperties();
        }
    }
}