using System.Collections.Generic;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
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
        ExpandableReorderableList defaultParameterItemsList;

        void OnEnable()
        {
            parametersWizard = target as ParametersWizard;
            
            parameterItemsList = new ExpandableReorderableList(new ParameterItemListDrawerBase(), serializedObject.FindProperty("parameterItems"));
            defaultParameterItemsList = new ExpandableReorderableList(new ParameterItemListDrawerBase(), serializedObject.FindProperty("defaultParameterItems"));
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = parametersWizard.EmoteWizardRoot;
            var expressionWizard = emoteWizardRoot.GetWizard<ExpressionWizard>();

            EmoteWizardGUILayout.SetupOnlyUI(parametersWizard, () =>
            {
                if (expressionWizard)
                {
                    if (GUILayout.Button("Repopulate Parameters"))
                    {
                        parametersWizard.parameterItems = new List<ParameterItem>();
                        parametersWizard.ForceRefreshParameters();
                    }
                }
            });

            using (ParameterItemDrawer.StartContext(emoteWizardRoot, false))
            {
                parameterItemsList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }
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

            using (ParameterItemDrawer.StartContext(emoteWizardRoot, true))
            {
                defaultParameterItemsList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
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