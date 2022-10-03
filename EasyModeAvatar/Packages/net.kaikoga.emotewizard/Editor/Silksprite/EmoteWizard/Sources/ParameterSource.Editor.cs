using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(ParameterSource))]
    public class ParameterSourceEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject.FindProperty(nameof(ParameterSource.parameterItem));

            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterItem.name)));
            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterItem.itemKind)));
            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterItem.defaultValue)));
            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterItem.saved)));

            EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterItem.usages)));

            serializedObject.ApplyModifiedProperties();
        }
    }
}