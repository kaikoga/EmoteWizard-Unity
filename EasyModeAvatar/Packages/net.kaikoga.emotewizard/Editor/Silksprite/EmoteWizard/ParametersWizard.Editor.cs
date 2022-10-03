using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Typed;
using Silksprite.EmoteWizard.DataObjects.Typed.DrawerContexts;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
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
            
            parameterItemsList = new ExpandableReorderableList<ParameterItem>(new ParameterItemListHeaderDrawer(), new ParameterItemDrawer(), "Parameter Items", ref parametersWizard.ParameterItems);
            defaultParameterItemsList = new ExpandableReorderableList<ParameterItem>(new ParameterItemListHeaderDrawer(), new ParameterItemDrawer(), "Default Parameter Items", ref parametersWizard.DefaultParameterItems);
            IsExpandedTracker.SetDefaultExpanded(parametersWizard.ParameterItems, false);
            IsExpandedTracker.SetDefaultExpanded(parametersWizard.DefaultParameterItems, false);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = parametersWizard.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(parametersWizard))
            {
                var expressionWizard = emoteWizardRoot.GetWizard<ExpressionWizard>();

                using (new ParameterItemDrawerContext(emoteWizardRoot, false).StartContext())
                {
                    parameterItemsList.DrawAsProperty(parametersWizard.ParameterItems, emoteWizardRoot.listDisplayMode);
                }

                using (new ParameterItemDrawerContext(emoteWizardRoot, false).StartContext())
                {
                    defaultParameterItemsList.DrawAsProperty(parametersWizard.DefaultParameterItems, emoteWizardRoot.listDisplayMode);
                }

                EmoteWizardGUILayout.RequireAnotherWizard(parametersWizard, expressionWizard,
                    () =>
                    {
                        if (GUILayout.Button("Manually Refresh Parameters"))
                        {
                            parametersWizard.RefreshParameters();
                            IsExpandedTracker.SetDefaultExpanded(parametersWizard.DefaultParameterItems, false);
                        }
                    });

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Expression Parameters"))
                    {
                        parametersWizard.BuildOutputAsset();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref parametersWizard.outputAsset);
                });
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "設定されたExpression Parametersを確認できます。");
    }
}