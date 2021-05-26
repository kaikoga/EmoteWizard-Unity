using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects.Internal;
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
            var vrcDefaultParameters = ExpressionParameterBuilder.ParameterStub.VrcDefaultParameters;
            var oldParameters = parametersWizard.outputAsset.parameters.ToList();
            var expressionParams = parametersWizard.ReplaceOrCreateOutputAsset(ref parametersWizard.outputAsset, "Expressions/GeneratedExprParams.asset");

            var builder = new ExpressionParameterBuilder();
            // create VRC default parameters entry
            if (parametersWizard.vrcDefaultParameters)
            {
                builder.Import(vrcDefaultParameters);
            }
            if (parametersWizard.outputAsset.parameters != null)
            {
                builder.Import(oldParameters);
            }

            foreach (var expressionItem in parametersWizard.GetComponent<ExpressionWizard>().expressionItems)
            {
                builder.FindOrCreate(expressionItem.parameter).AddUsage(expressionItem.value);
            }

            // override VRC default parameters with default values
            if (parametersWizard.vrcDefaultParameters)
            {
                builder.Import(vrcDefaultParameters);
            }

            expressionParams.parameters = builder.Export();

            AssetDatabase.SaveAssets();
        }
    }
}