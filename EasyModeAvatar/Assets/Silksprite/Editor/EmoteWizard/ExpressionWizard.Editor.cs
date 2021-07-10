using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(ExpressionWizard))]
    public class ExpressionWizardEditor : AnimationWizardBaseEditor
    {
        ExpressionWizard expressionWizard;

        ExpandableReorderableList expressionItemsList;

        void OnEnable()
        {
            expressionWizard = target as ExpressionWizard;
            
            expressionItemsList = new ExpandableReorderableList(new ExpressionItemListDrawerBase(), serializedObject.FindProperty("expressionItems"));
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            var emoteWizardRoot = expressionWizard.EmoteWizardRoot;

            EmoteWizardGUILayout.SetupOnlyUI(expressionWizard, () =>
            {
                if (GUILayout.Button("Reset Expression Items"))
                {
                    RepopulateDefaultExpressionItems();
                }
            });

            using (ExpressionItemDrawer.StartContext(emoteWizardRoot))
            {
                expressionItemsList.DrawAsProperty(emoteWizardRoot.listDisplayMode);
            }

            EditorGUILayout.PropertyField(serializedObj.FindProperty("buildAsSubAsset"));
            EditorGUILayout.PropertyField(serializedObj.FindProperty("defaultPrefix"));

            if (GUILayout.Button("Populate Expression Items"))
            {
                PopulateDefaultExpressionItems();
            }

            EmoteWizardGUILayout.OutputUIArea(() =>
            {
                if (GUILayout.Button("Generate Expression Menu"))
                {
                    BuildExpressionMenu();
                }
                EditorGUILayout.PropertyField(serializedObj.FindProperty("outputAsset"));
            });

            serializedObj.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "Expression Menuの設定を一括で行い、アセットを出力します。\nここで入力した値は他のWizardに自動的に引き継がれます。");
        }

        void RepopulateDefaultExpressionItems()
        {
            expressionWizard.expressionItems = new List<ExpressionItem>();
            PopulateDefaultExpressionItems();
        }

        void PopulateDefaultExpressionItems()
        {
            var icon = VrcSdkAssetLocator.PersonDance();
            var expressionItems = Enumerable.Range(1, 8)
                .Select(i => ExpressionItem.PopulateDefault(icon, expressionWizard.defaultPrefix, i));
            expressionWizard.expressionItems.AddRange(expressionItems);
            expressionWizard.expressionItems = expressionWizard.expressionItems
                .GroupBy(item => item.path)
                .Select(g => g.First())
                .ToList();
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
                var controls = group.Items.Select(item => item.ToControl(path => menus[path])).ToList();
                menus[group.Path].controls = controls;
            }

            AssetDatabase.SaveAssets();
        }
    }
}