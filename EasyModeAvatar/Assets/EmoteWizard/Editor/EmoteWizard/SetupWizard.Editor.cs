using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard
{
    [CustomEditor(typeof(SetupWizard))]
    public class SetupWizardEditor : Editor
    {
        SetupWizard setupWizard;

        void OnEnable()
        {
            setupWizard = target as SetupWizard;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isSetupMode"), new GUIContent("Enable Setup Only UI"));
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Generate Wizards"))
            {
                setupWizard.EnsureComponent<AvatarWizard>();
                setupWizard.EnsureComponent<ExpressionWizard>();
                setupWizard.EnsureComponent<ParametersWizard>();
                setupWizard.EnsureComponent<GestureWizard>();
                setupWizard.EnsureComponent<FxWizard>();
            }
            if (GUILayout.Button("Complete setup and remove me"))
            {
                DestroyImmediate(setupWizard);
            }
        }
    }
}