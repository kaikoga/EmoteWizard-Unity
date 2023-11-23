using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(ExpressionWizard))]
    public class ExpressionWizardEditor : Editor
    {
        ExpressionWizard _wizard;

        SerializedProperty _serializedBuildAsSubAsset;
        SerializedProperty _serializedOutputAsset;

        void OnEnable()
        {
            _wizard = (ExpressionWizard)target;

            _serializedBuildAsSubAsset = serializedObject.FindProperty(nameof(ExpressionWizard.buildAsSubAsset));
            _serializedOutputAsset = serializedObject.FindProperty(nameof(ExpressionWizard.outputAsset));
        }

        public override void OnInspectorGUI()
        {
            var env = _wizard.Environment;

            EditorGUILayout.PropertyField(_serializedBuildAsSubAsset);

            EmoteWizardGUILayout.OutputUIArea(() =>
            {
                if (GUILayout.Button("Generate Expression Menu"))
                {
                    _wizard.GetContext().BuildOutputAsset();
                }

                EditorGUILayout.PropertyField(_serializedOutputAsset);
            });

            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(env, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "Expression Menuを生成します。");

    }
}