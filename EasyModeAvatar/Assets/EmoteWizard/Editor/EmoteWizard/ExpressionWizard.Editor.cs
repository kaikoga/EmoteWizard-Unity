using System.Collections.Generic;
using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects;
using EmoteWizard.Extensions;
using EmoteWizard.Tools;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
using static EmoteWizard.Extensions.EditorUITools;

namespace EmoteWizard
{
    [CustomEditor(typeof(ExpressionWizard))]
    public class ExpressionWizardEditor : AnimationWizardBaseEditor
    {
        ExpressionWizard expressionWizard;

        ExpandableReorderableList expressionItemsList;

        void OnEnable()
        {
            expressionWizard = target as ExpressionWizard;
            
            expressionItemsList = new ExpandableReorderableList(serializedObject, serializedObject.FindProperty("expressionItems"));
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = this.serializedObject;

            SetupOnlyUI(expressionWizard, () =>
            {
                if (GUILayout.Button("Reset Expression Items"))
                {
                    RepopulateDefaultExpressionItems();
                }
            });

            expressionItemsList.DrawAsProperty(expressionWizard.EmoteWizardRoot.useReorderUI);

            EditorGUILayout.PropertyField(serializedObj.FindProperty("buildAsSubAsset"));
            EditorGUILayout.PropertyField(serializedObj.FindProperty("defaultPrefix"));

            if (GUILayout.Button("Populate Expression Items"))
            {
                PopulateDefaultExpressionItems();
            }

            OutputUIArea(expressionWizard, () =>
            {
                if (GUILayout.Button("Generate Expression Menu"))
                {
                    BuildExpressionMenu();
                }
                EditorGUILayout.PropertyField(serializedObj.FindProperty("outputAsset"));
            });

            serializedObj.ApplyModifiedProperties();
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