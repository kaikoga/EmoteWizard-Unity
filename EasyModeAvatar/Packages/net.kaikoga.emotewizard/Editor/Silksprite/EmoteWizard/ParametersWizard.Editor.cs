using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(ParametersWizard))]
    public class ParametersWizardEditor : Editor
    {
        ParametersWizard _wizard;

        SerializedProperty _serializedOutputAsset;

        void OnEnable()
        {
            _wizard = (ParametersWizard) target;

            _serializedOutputAsset = serializedObject.FindProperty(nameof(ParametersWizard.outputAsset));
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _wizard.EmoteWizardRoot;

            EmoteWizardGUILayout.OutputUIArea(() =>
            {
                if (GUILayout.Button("Generate Expression Parameters"))
                {
                    _wizard.BuildOutputAsset();
                }

                EditorGUILayout.PropertyField(_serializedOutputAsset);
            });
            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "Expression Parametersを管理するために必要なコンポーネントです。");
    }
}