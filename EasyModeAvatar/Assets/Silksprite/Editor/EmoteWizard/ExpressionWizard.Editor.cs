using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(ExpressionWizard))]
    public class ExpressionWizardEditor : Editor
    {
        ExpressionWizard expressionWizard;

        ExpandableReorderableList<ExpressionItem> expressionItemsList;

        void OnEnable()
        {
            expressionWizard = (ExpressionWizard) target;
            
            expressionItemsList = new ExpandableReorderableList<ExpressionItem>(new ExpressionItemListHeaderDrawer(), new ExpressionItemDrawer(), "Legacy Expression Items", ref expressionWizard.legacyExpressionItems);
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(expressionWizard))
            {
                var emoteWizardRoot = expressionWizard.EmoteWizardRoot;

                if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

                EmoteWizardGUILayout.SetupOnlyUI(expressionWizard, () =>
                {
                    if (GUILayout.Button("Reset Expression Items"))
                    {
                        expressionWizard.RepopulateDefaultExpressionItems();
                    }
                });

                TypedGUILayout.Toggle("Build As Sub Asset", ref expressionWizard.buildAsSubAsset);

                if (expressionWizard.legacyExpressionItems.Any())
                {
                    using (new ExpressionItemDrawerContext(emoteWizardRoot).StartContext())
                    {
                        expressionItemsList.DrawAsProperty(expressionWizard.legacyExpressionItems, emoteWizardRoot.listDisplayMode);
                    }
                    TypedGUILayout.TextField("Default Prefix", ref expressionWizard.defaultPrefix);
                    if (GUILayout.Button("Populate Default Expression Items"))
                    {
                        expressionWizard.PopulateDefaultExpressionItems();
                    }
                    if (GUILayout.Button("Group by Folder"))
                    {
                        GroupItemsByFolder();
                    }
                    if (GUILayout.Button("Migrate to Data Source"))
                    {
                        MigrateToDataSource();
                    }
                }
                if (GUILayout.Button("Add Expression Item Source"))
                {
                    expressionWizard.AddChildComponentAndSelect<ExpressionItemSource>();
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Expression Menu"))
                    {
                        expressionWizard.BuildOutputAsset();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref expressionWizard.outputAsset);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "Expression Menuの設定を一括で行い、アセットを出力します。\nここで入力した値は他のWizardに自動的に引き継がれます。\n項目名を半角スラッシュで区切るとサブメニューを作成できます。");
            }
        }

        void GroupItemsByFolder()
        {
            expressionWizard.legacyExpressionItems = expressionWizard.legacyExpressionItems
                .GroupBy(item => item.Folder)
                .SelectMany(group => group)
                .ToList();
        }

        void MigrateToDataSource()
        {
            var source = expressionWizard.AddChildComponent<ExpressionItemSource>();
            source.expressionItems = expressionWizard.legacyExpressionItems.ToList();
            expressionWizard.legacyExpressionItems.Clear();
            source.defaultPrefix = expressionWizard.defaultPrefix;
        }
    }
}