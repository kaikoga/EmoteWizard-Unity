using EmoteWizard.Base;
using EmoteWizard.Collections;
using EmoteWizard.Extensions;
using EmoteWizard.UI;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard
{
    [CustomEditor(typeof(ParametersWizard))]
    public class ParametersWizardEditor : AnimationWizardBaseEditor
    {
        ParametersWizard parametersWizard;
        ExpandableReorderableList parameterItemsList;

        void OnEnable()
        {
            parametersWizard = target as ParametersWizard;
            
            parameterItemsList = new ExpandableReorderableList(serializedObject,
                serializedObject.FindProperty("parameterItems"),
                "Parameter Items",
                new ParameterItemListDrawerBase(),
                (property, index) => property.FindPropertyRelative("name").stringValue);
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = parametersWizard.EmoteWizardRoot;
            var expressionWizard = parametersWizard.GetComponent<ExpressionWizard>();

            EmoteWizardGUILayout.SetupOnlyUI(parametersWizard, () =>
            {
                if (expressionWizard)
                {
                    if (GUILayout.Button("Repopulate Parameters"))
                    {
                        parametersWizard.parameterItems.Clear();
                        parametersWizard.ForceRefreshParameters();
                    }
                }
            });

            EditorGUILayout.PropertyField(serializedObj.FindProperty("vrcDefaultParameters"));
            EmoteWizardGUILayout.RequireAnotherWizard(parametersWizard, expressionWizard,
                () =>
                {
                    if (GUILayout.Button("Collect Parameters (auto)"))
                    {
                        parametersWizard.ForceRefreshParameters();
                    }
                });

            parameterItemsList.DrawAsProperty(emoteWizardRoot.listDisplayMode);

            EmoteWizardGUILayout.OutputUIArea(() =>
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
            var expressionParams = parametersWizard.ReplaceOrCreateOutputAsset(ref parametersWizard.outputAsset, "Expressions/@@@Generated@@@ExprParams.asset");

            expressionParams.parameters = parametersWizard.ToParameters();

            AssetDatabase.SaveAssets();
        }

    }
}