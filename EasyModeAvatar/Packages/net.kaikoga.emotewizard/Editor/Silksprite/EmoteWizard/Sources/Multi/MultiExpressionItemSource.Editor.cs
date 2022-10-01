using System.Linq;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.Sources.Multi.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Multi
{
    [CustomEditor(typeof(MultiExpressionItemSource))]
    public class MultiExpressionItemSourceEditor : Editor
    {
        MultiExpressionItemSource _multiExpressionItemSource;

        ExpandableReorderableList<ExpressionItem> _expressionItemsList;

        void OnEnable()
        {
            _multiExpressionItemSource = (MultiExpressionItemSource)target;
            
            _expressionItemsList = new ExpandableReorderableList<ExpressionItem>(new ExpressionItemListHeaderDrawer(), new ExpressionItemDrawer(), "Expression Items", ref _multiExpressionItemSource.expressionItems);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _multiExpressionItemSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_multiExpressionItemSource))
            {
                EmoteWizardGUILayout.SetupOnlyUI(_multiExpressionItemSource, () =>
                {
                    if (GUILayout.Button("Reset Expression Items"))
                    {
                        _multiExpressionItemSource.RepopulateDefaultExpressionItems();
                    }
                });

                using (new ExpressionItemDrawerContext(emoteWizardRoot).StartContext())
                {
                    _expressionItemsList.DrawAsProperty(_multiExpressionItemSource.expressionItems, emoteWizardRoot.listDisplayMode);
                }

                TypedGUILayout.TextField("Default Prefix", ref _multiExpressionItemSource.defaultPrefix);
                if (GUILayout.Button("Populate Default Expression Items"))
                {
                    _multiExpressionItemSource.PopulateDefaultExpressionItems();
                }

                if (GUILayout.Button("Group by Folder (Sort)"))
                {
                    GroupItemsByFolder();
                }
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial2);
        }

        void GroupItemsByFolder()
        {
            _multiExpressionItemSource.expressionItems = _multiExpressionItemSource.expressionItems
                .GroupBy(item => item.Folder)
                .SelectMany(group => group)
                .ToList();
        }
        
        static string Tutorial => 
            string.Join("\n",
                "Expressions MenuとExpression Parameterに追加する項目を設定します。");
        static string Tutorial2 => 
            string.Join("\n",
                "項目名を半角スラッシュで区切るとサブメニューを作成できます。");
    }
}