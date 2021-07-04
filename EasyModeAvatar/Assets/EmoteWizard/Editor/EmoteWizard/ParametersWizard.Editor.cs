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
                new ParameterItemListHeaderDrawer());
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = parametersWizard.EmoteWizardRoot;

            EmoteWizardGUILayout.SetupOnlyUI(parametersWizard, () =>
            {
                if (GUILayout.Button("Repopulate Parameters"))
                {
                    parametersWizard.parameterItems.Clear();
                    parametersWizard.ForceRefreshParameters();
                }
            });

            EditorGUILayout.PropertyField(serializedObj.FindProperty("vrcDefaultParameters"));
            if (GUILayout.Button("Collect Parameters (auto)"))
            {
                parametersWizard.ForceRefreshParameters();
            }

            parameterItemsList.DrawAsProperty(emoteWizardRoot.useReorderUI);

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