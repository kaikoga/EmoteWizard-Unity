using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
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

        void OnEnable()
        {
            var serializedItem = serializedObject.FindProperty(nameof(ParameterSource.parameterItem));

            _serializedName = serializedItem.FindPropertyRelative(nameof(ParameterItem.name));
            _serializedItemKind = serializedItem.FindPropertyRelative(nameof(ParameterItem.itemKind));
            _serializedDefaultValue = serializedItem.FindPropertyRelative(nameof(ParameterItem.defaultValue));
            _serializedSaved = serializedItem.FindPropertyRelative(nameof(ParameterItem.saved));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedName);
            EditorGUILayout.PropertyField(_serializedItemKind);
            EditorGUILayout.PropertyField(_serializedDefaultValue);
            EditorGUILayout.PropertyField(_serializedSaved);

            serializedObject.ApplyModifiedProperties();
            
            EmoteWizardGUILayout.Tutorial(((ParameterSource)target).CreateEnv(), Tutorial);
        }
        
        static string Tutorial =>
            string.Join("\n",
                "外部アセットが利用するExpression Parameterを任意に登録します。");
    }
}