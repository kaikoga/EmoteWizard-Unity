using System.Collections.Generic;
using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
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

            var groups = Enumerable.Empty<VRCExpressionParameters.Parameter>()
                .Concat(parametersWizard.outputAsset.parameters ?? Enumerable.Empty<VRCExpressionParameters.Parameter>() )
                .Concat(new List<VRCExpressionParameters.Parameter>
                {
                    new VRCExpressionParameters.Parameter
                    {
                        defaultValue = 0,
                        name = "VRCEmote",
                        saved = false,
                        valueType = VRCExpressionParameters.ValueType.Int
                    },
                    new VRCExpressionParameters.Parameter
                    {
                        defaultValue = 0,
                        name = "VRCFaceBlendH",
                        saved = false,
                        valueType = VRCExpressionParameters.ValueType.Float
                    },
                    new VRCExpressionParameters.Parameter
                    {
                        defaultValue = 0,
                        name = "VRCFaceBlendV",
                        saved = false,
                        valueType = VRCExpressionParameters.ValueType.Float
                    }
                }).Concat(parametersWizard.GetComponent<ExpressionWizard>().expressionItems
                    .Select(expressionItem => new VRCExpressionParameters.Parameter
                    {
                        defaultValue = 0,
                        name = expressionItem.parameter,
                        saved = false,
                        valueType = VRCExpressionParameters.ValueType.Int
                    }))
                .GroupBy(parameter => parameter.name)
                .Select(group => group.First());

            expressionParams.parameters = groups.ToArray();

            AssetDatabase.SaveAssets();
        }
    }
}