using Silksprite.EmoteWizard.Extensions;
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
            _wizard = (ExpressionWizard) target;

            _serializedBuildAsSubAsset = serializedObject.FindProperty(nameof(ExpressionWizard.buildAsSubAsset));
            _serializedOutputAsset = serializedObject.FindProperty(nameof(ExpressionWizard.outputAsset));
        }

        public override void OnInspectorGUI()
        {
            var context = _wizard.Context;

            EditorGUILayout.PropertyField(_serializedBuildAsSubAsset);

            EmoteWizardGUILayout.OutputUIArea(() =>
            {
                if (GUILayout.Button("Generate Expression Menu"))
                {
                    _wizard.BuildOutputAsset();
                }

                EditorGUILayout.PropertyField(_serializedOutputAsset);
            });

            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(context, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "Expression Menuを生成します。");

    }
}