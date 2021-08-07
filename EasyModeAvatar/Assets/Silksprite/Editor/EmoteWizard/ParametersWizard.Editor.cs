using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(ParametersWizard))]
    public class ParametersWizardEditor : Editor
    {
        ParametersWizard parametersWizard;
        ExpandableReorderableList<ParameterItem> parameterItemsList;
        ExpandableReorderableList<ParameterItem> defaultParameterItemsList;

        void OnEnable()
        {
            parametersWizard = (ParametersWizard) target;
            
            parameterItemsList = new ExpandableReorderableList<ParameterItem>(new ParameterItemListHeaderDrawer(), new ParameterItemDrawer(), "Parameter Items", ref parametersWizard.parameterItems);
            defaultParameterItemsList = new ExpandableReorderableList<ParameterItem>(new ParameterItemListHeaderDrawer(), new ParameterItemDrawer(), "Default Parameter Items", ref parametersWizard.defaultParameterItems);
            IsExpandedTracker.SetDefaultExpanded(parametersWizard.defaultParameterItems, false);
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(parametersWizard))
            {
                var emoteWizardRoot = parametersWizard.EmoteWizardRoot;
                var expressionWizard = emoteWizardRoot.GetWizard<ExpressionWizard>();

                EmoteWizardGUILayout.SetupOnlyUI(parametersWizard, () =>
                {
                    if (expressionWizard)
                    {
                        if (GUILayout.Button("Repopulate Parameters"))
                        {
                            parametersWizard.RepopulateParameters();
                        }
                    }
                });

                using (new ParameterItemDrawerContext(emoteWizardRoot, false).StartContext())
                {
                    parameterItemsList.DrawAsProperty(parametersWizard.parameterItems, emoteWizardRoot.listDisplayMode);
                }

                if (parameterItemsList.IsExpanded)
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

                using (new ParameterItemDrawerContext(emoteWizardRoot, true).StartContext())
                {
                    defaultParameterItemsList.DrawAsProperty(parametersWizard.defaultParameterItems, emoteWizardRoot.listDisplayMode);
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Expression Parameters"))
                    {
                        parametersWizard.BuildOutputAsset();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref parametersWizard.outputAsset);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "Expression Parametersの設定を行います。\nここに登録されているパラメータはAnimator Controllerにも自動的に追加されます。\nパラメータを消費する他のアセットと連携する場合は、ここを調整して必要なパラメータを追加してください。");
            }
        }
    }
}