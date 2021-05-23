using System.Collections.Generic;
using System.Linq;
using EmoteWizard.Base;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using static EmoteWizard.Extensions.EditorUITools;
using static EmoteWizard.Tools.EmoteWizardEditorTools;

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

        VRCExpressionParameters RebuildOrCreateExpressionParameters(string defaultRelativePath)
        {
            var outputAsset = parametersWizard.outputAsset;
            var path = outputAsset ? AssetDatabase.GetAssetPath(outputAsset) : parametersWizard.EmoteWizardRoot.GeneratedAssetPath(defaultRelativePath);

            EnsureDirectory(path);
            var expressionParameters = CreateInstance<VRCExpressionParameters>();
            AssetDatabase.CreateAsset(expressionParameters, path);
            return expressionParameters;
        }

        void BuildExpressionParameters()
        {
            var expressionParams = RebuildOrCreateExpressionParameters("Expressions/GeneratedExprParams.asset");

            var groups = Enumerable.Empty<VRCExpressionParameters.Parameter>()
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
                });

            expressionParams.parameters = groups.ToArray();

            AssetDatabase.SaveAssets();
            parametersWizard.outputAsset = expressionParams;
        }
    }
}