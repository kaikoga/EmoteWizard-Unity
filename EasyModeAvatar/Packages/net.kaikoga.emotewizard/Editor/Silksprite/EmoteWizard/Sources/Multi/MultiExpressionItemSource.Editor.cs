using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Typed;
using Silksprite.EmoteWizard.DataObjects.Typed.DrawerContexts;
using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
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

            using (new ObjectChangeScope(_multiExpressionItemSource))
            {
                using (new ExpressionItemDrawerContext(emoteWizardRoot).StartContext())
                {
                    _expressionItemsList.DrawAsProperty(_multiExpressionItemSource.expressionItems, emoteWizardRoot.listDisplayMode);
                }

                if (GUILayout.Button("Explode"))
                {
                    SourceExploder.Explode(_multiExpressionItemSource);
                }
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial2);
        }

        static string Tutorial => 
            string.Join("\n",
                "Expressions MenuとExpression Parameterに追加する項目を設定します。");
        static string Tutorial2 => 
            string.Join("\n",
                "項目名を半角スラッシュで区切るとサブメニューを作成できます。");
    }
}