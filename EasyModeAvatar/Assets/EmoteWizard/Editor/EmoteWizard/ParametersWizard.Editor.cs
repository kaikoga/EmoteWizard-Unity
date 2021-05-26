using EmoteWizard.Base;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Extensions.EditorUITools;

namespace EmoteWizard
{
    [CustomEditor(typeof(ParametersWizard))]
    public class ParametersWizardEditor : AnimationWizardBaseEditor
    {
        ParametersWizard parametersWizard;

        void OnEnable()
        {
            parametersWizard = target as ParametersWizard;
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = this.serializedObject;

            EditorGUILayout.PropertyField(serializedObj.FindProperty("vrcDefaultParameters"));

            OutputUIArea(parametersWizard, () =>
            {
                if (GUILayout.Button("Generate Expression Parameters"))
                {
                    BuildExpressionParameters();
                }
                EditorGUILayout.PropertyField(serializedObj.FindProperty("outputAsset"));
            });

            serializedObj.ApplyModifiedProperties();
        }

        void BuildExpressionParameters()
        {
            var expressionParams = parametersWizard.ReplaceOrCreateOutputAsset(ref parametersWizard.outputAsset, "Expressions/GeneratedExprParams.asset");

            expressionParams.parameters = parametersWizard.ToParameters();

            AssetDatabase.SaveAssets();
        }

    }
}