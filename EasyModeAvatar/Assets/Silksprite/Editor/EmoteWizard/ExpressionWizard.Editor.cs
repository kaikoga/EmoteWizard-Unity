using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(ExpressionWizard))]
    public class ExpressionWizardEditor : AnimationWizardBaseEditor
    {
        ExpressionWizard expressionWizard;

        ExpandableReorderableList<ExpressionItem> expressionItemsList;

        void OnEnable()
        {
            expressionWizard = (ExpressionWizard) target;
            
            expressionItemsList = new ExpandableReorderableList<ExpressionItem>(new ExpressionItemListDrawer(), new ExpressionItemDrawer(), "Expression Items", ref expressionWizard.expressionItems);
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(expressionWizard))
            {
                var emoteWizardRoot = expressionWizard.EmoteWizardRoot;

                EmoteWizardGUILayout.SetupOnlyUI(expressionWizard, () =>
                {
                    if (GUILayout.Button("Reset Expression Items"))
                    {
                        SetupWizardUtils.RepopulateDefaultExpressionItems(expressionWizard);
                    }
                });

                using (ExpressionItemDrawer.StartContext(emoteWizardRoot))
                {
                    expressionItemsList.DrawAsProperty(expressionWizard.expressionItems, emoteWizardRoot.listDisplayMode);
                }

                TypedGUILayout.Toggle("Build As Sub Asset", ref expressionWizard.buildAsSubAsset);
                TypedGUILayout.TextField("Default Prefix", ref expressionWizard.defaultPrefix);

                if (GUILayout.Button("Populate Default Expression Items"))
                {
                    SetupWizardUtils.PopulateDefaultExpressionItems(expressionWizard);
                }

                if (GUILayout.Button("Group by Folder"))
                {
                    GroupItemsByFolder();
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Expression Menu"))
                    {
                        BuildExpressionMenu();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref expressionWizard.outputAsset);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "Expression Menuの設定を一括で行い、アセットを出力します。\nここで入力した値は他のWizardに自動的に引き継がれます。\n項目名を半角スラッシュで区切るとサブメニューを作成できます。");
            }
        }

        void BuildExpressionMenu()
        {
            var expressionMenu = expressionWizard.ReplaceOrCreateOutputAsset(ref expressionWizard.outputAsset, "Expressions/@@@Generated@@@ExprMenu.asset");

            var rootItemPath = AssetDatabase.GetAssetPath(expressionMenu);
            var rootPath = $"{rootItemPath.Substring(0, rootItemPath.Length - 6)}/";

            var groups = expressionWizard.GroupExpressionItems().ToList();

            var menus = new Dictionary<string, VRCExpressionsMenu>();

            // populate folders first
            foreach (var group in groups)
            {
                if (group.Path == "")
                {
                    menus[group.Path] = expressionMenu;
                    EditorUtility.SetDirty(expressionMenu);
                }
                else if (expressionWizard.buildAsSubAsset)
                {
                    var childMenu = CreateInstance<VRCExpressionsMenu>();
                    AssetDatabase.AddObjectToAsset(childMenu, rootItemPath);
                    childMenu.name = group.Path;
                    menus[group.Path] = childMenu;
                }
                else
                {
                    var childMenu = CreateInstance<VRCExpressionsMenu>();
                    var childPath = $"{rootPath}{group.Path}.asset";
                    expressionWizard.ReplaceOrCreateOutputAsset(ref childMenu, childPath);
                    menus[group.Path] = childMenu;
                }
            }
            
            foreach (var group in groups)
            {
                var controls = group.Items
                    .Select(item => item.ToControl(path => menus.TryGetValue(path, out var v) ? v : null))
                    .ToList();
                menus[group.Path].controls = controls;
            }

            AssetDatabase.SaveAssets();
        }

        void GroupItemsByFolder()
        {
            expressionWizard.expressionItems = expressionWizard.expressionItems
                .GroupBy(item => item.Folder)
                .SelectMany(group => group)
                .ToList();
        }
    }
}