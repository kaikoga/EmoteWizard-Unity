using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Wizards
{
    [CustomEditor(typeof(EmoteItemWizard))]
    public class EmoteItemWizardEditor : Editor
    {
        SerializedProperty _serializedHasExpressionItemSource;
        SerializedProperty _serializedItemPath;
        SerializedProperty _serializedHasGroupName;
        SerializedProperty _serializedGroupName;
        SerializedProperty _serializedHasParameterName;
        SerializedProperty _serializedParameterName;

        EmoteItemWizard _wizard;

        void OnEnable()
        {
            _serializedHasExpressionItemSource = serializedObject.FindProperty(nameof(EmoteItemWizard.hasExpressionItemSource));
            _serializedItemPath = serializedObject.FindProperty(nameof(EmoteItemWizard.itemPath));
            _serializedHasGroupName = serializedObject.FindProperty(nameof(EmoteItemWizard.hasGroupName));
            _serializedGroupName = serializedObject.FindProperty(nameof(EmoteItemWizard.groupName));
            _serializedHasParameterName = serializedObject.FindProperty(nameof(EmoteItemWizard.hasParameterName));
            _serializedParameterName = serializedObject.FindProperty(nameof(EmoteItemWizard.parameterName));
            _wizard = (EmoteItemWizard)target;
        }

        static bool IsInvalidPathInput(string value) => string.IsNullOrWhiteSpace(value) || value.StartsWith("/") || value.EndsWith("/");
        static bool IsInvalidParameterInput(string value) => string.IsNullOrWhiteSpace(value) || value.Contains("/");

        public override void OnInspectorGUI()
        {
            var disableAddButton = false;

            void ItemPath()
            {
                var isInvalidNameInput = IsInvalidPathInput(_serializedItemPath.stringValue);
                disableAddButton |= isInvalidNameInput;
                using (new InvalidValueScope(isInvalidNameInput))
                {
                    EditorGUILayout.PropertyField(_serializedItemPath, new GUIContent("Item Path"));
                }
            }

            void GroupName()
            {
                EditorGUILayout.PropertyField(_serializedHasGroupName, new GUIContent("Set Group Name"));
                if (_serializedHasGroupName.boolValue)
                {
                    var isInvalidNameInput = IsInvalidPathInput(_serializedGroupName.stringValue);
                    disableAddButton |= isInvalidNameInput;
                    using (new EditorGUI.IndentLevelScope())
                    using (new InvalidValueScope(isInvalidNameInput))
                    {
                        EditorGUILayout.PropertyField(_serializedGroupName, new GUIContent("Group Name"));
                    }
                }
            }

            void ParameterName()
            {
                EditorGUILayout.PropertyField(_serializedHasParameterName, new GUIContent("Set Parameter Name"));
                if (_serializedHasParameterName.boolValue)
                {
                    var isInvalidNameInput = IsInvalidParameterInput(_serializedParameterName.stringValue);
                    disableAddButton |= isInvalidNameInput;
                    using (new EditorGUI.IndentLevelScope(1))
                    using (new InvalidValueScope(isInvalidNameInput))
                    {
                        EditorGUILayout.PropertyField(_serializedParameterName, new GUIContent("Parameter Name"));
                    }
                }
            }

            ItemPath();
            GroupName();
            ParameterName();

            EditorGUILayout.PropertyField(_serializedHasExpressionItemSource, new GUIContent("Use Expression Item Source"));

            serializedObject.ApplyModifiedProperties();

            using (new EditorGUI.DisabledScope(disableAddButton))
            {
                if (GUILayout.Button("Add")) WizardExploder.Explode(_wizard);
            }

            EmoteWizardGUILayout.Tutorial(_wizard.CreateEnv(), Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "Emote Wizardに登録するデータの入力欄を生成します。");
    }
}