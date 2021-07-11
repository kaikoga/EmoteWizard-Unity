using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(ParametersWizard))]
    public class ParametersWizardEditor : AnimationWizardBaseEditor
    {
        ParametersWizard parametersWizard;
        ExpandableReorderableList parameterItemsList;

        void OnEnable()
        {
            parametersWizard = target as ParametersWizard;
            
            parameterItemsList = new ExpandableReorderableList(new ParameterItemListDrawerBase(), serializedObject.FindProperty("parameterItems"));
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

            parameterItemsList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            if (parameterItemsList.serializedProperty.isExpanded)
            {
                EmoteWizardGUILayout.RequireAnotherWizard(parametersWizard, expressionWizard,
                    () =>
                    {
                        if (GUILayout.Button("Collect Parameters (auto)"))
                        {
                            parametersWizard.ForceRefreshParameters();
                        }
                    });
            }

            EmoteWizardGUILayout.OutputUIArea(() =>
            {
                if (GUILayout.Button("Generate Expression Parameters"))
                {
                    BuildExpressionParameters();
                }
                EditorGUILayout.PropertyField(serializedObj.FindProperty("outputAsset"));
            });

            serializedObj.ApplyModifiedProperties();
            
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "Expression Parametersの設定を行います。\nここに登録されているパラメータはAnimator Controllerにも自動的に追加されます。\nパラメータを消費する他のアセットと連携する場合は、ここを調整して必要なパラメータを追加してください。");
        }

        void BuildExpressionParameters()
        {
            var expressionParams = parametersWizard.ReplaceOrCreateOutputAsset(ref parametersWizard.outputAsset, "Expressions/@@@Generated@@@ExprParams.asset");

            expressionParams.parameters = parametersWizard.ToParameters();

            AssetDatabase.SaveAssets();
        }

    }
}