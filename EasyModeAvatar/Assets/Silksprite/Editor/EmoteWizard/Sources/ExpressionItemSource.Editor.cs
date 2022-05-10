using System.Linq;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(ExpressionItemSource))]
    public class ExpressionItemSourceEditor : Editor
    {
        ExpressionItemSource _expressionItemSource;

        ExpandableReorderableList<ExpressionItem> _expressionItemsList;

        void OnEnable()
        {
            _expressionItemSource = (ExpressionItemSource)target;
            
            _expressionItemsList = new ExpandableReorderableList<ExpressionItem>(new ExpressionItemListHeaderDrawer(), new ExpressionItemDrawer(), "Expression Items", ref _expressionItemSource.expressionItems);
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(_expressionItemSource))
            {
                var emoteWizardRoot = _expressionItemSource.EmoteWizardRoot;

                if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

                EmoteWizardGUILayout.SetupOnlyUI(_expressionItemSource, () =>
                {
                    if (GUILayout.Button("Reset Expression Items"))
                    {
                        _expressionItemSource.RepopulateDefaultExpressionItems();
                    }
                });

                using (new ExpressionItemDrawerContext(emoteWizardRoot).StartContext())
                {
                    _expressionItemsList.DrawAsProperty(_expressionItemSource.expressionItems, emoteWizardRoot.listDisplayMode);
                }

                TypedGUILayout.TextField("Default Prefix", ref _expressionItemSource.defaultPrefix);
                if (GUILayout.Button("Populate Default Expression Items"))
                {
                    _expressionItemSource.PopulateDefaultExpressionItems();
                }

                if (GUILayout.Button("Group by Folder"))
                {
                    GroupItemsByFolder();
                }

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "Expression Menuの設定を一括で行い、アセットを出力します。\nここで入力した値は他のWizardに自動的に引き継がれます。\n項目名を半角スラッシュで区切るとサブメニューを作成できます。");
            }
        }

        void GroupItemsByFolder()
        {
            _expressionItemSource.expressionItems = _expressionItemSource.expressionItems
                .GroupBy(item => item.Folder)
                .SelectMany(group => group)
                .ToList();
        }
    }
}