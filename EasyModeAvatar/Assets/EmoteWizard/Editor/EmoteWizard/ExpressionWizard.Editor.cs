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
using static EmoteWizard.Tools.EmoteWizardEditorTools;

namespace EmoteWizard
{
    [CustomEditor(typeof(ExpressionWizard))]
    public class ExpressionWizardEditor : AnimationWizardBaseEditor
    {
        ExpressionWizard expressionWizard;

        void OnEnable()
        {
            expressionWizard = target as ExpressionWizard;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("expressionItems"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("outputAsset"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("buildAsSubAsset"));

            SetupOnlyUI(expressionWizard, () =>
            {
                if (GUILayout.Button("Repopulate Expression Items"))
                {
                    RepopulateDefaultExpressionItems(expressionWizard);
                }
            });
            if (GUILayout.Button("Generate Expression Menu"))
            {
                BuildExpressionMenu();
            }
        }

        static void RepopulateDefaultExpressionItems(ExpressionWizard expressionWizard)
        {
            var icon = VrcSdkAssetLocator.PersonDance();
            var expressionItems = Enumerable.Range(1, 8)
                .Select(i => ExpressionItem.PopulateDefault(icon, i))
                .ToList();
            expressionWizard.expressionItems = expressionItems;
        }

        VRCExpressionsMenu RebuildOrCreateExpressionsMenu(string defaultRelativePath)
        {
            var outputAsset = expressionWizard.outputAsset;
            var path = outputAsset ? AssetDatabase.GetAssetPath(outputAsset) : expressionWizard.EmoteWizardRoot.GeneratedAssetPath(defaultRelativePath);

            EnsureDirectory(path);
            var expressionsMenu = CreateInstance<VRCExpressionsMenu>();
            AssetDatabase.CreateAsset(expressionsMenu, path);
            return expressionsMenu;
        }

        void BuildExpressionMenu()
        {
            var expressionMenu = RebuildOrCreateExpressionsMenu("Expressions/GeneratedExprMenu.asset");
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
                    EnsureDirectory(childPath);
                    AssetDatabase.CreateAsset(childMenu, childPath);
                    menus[group.Path] = childMenu;
                    EditorUtility.SetDirty(childMenu);
                }
            }
            
            foreach (var group in groups)
            {
                var controls = group.Items.Select(item => item.ToControl(path => menus[path])).ToList();
                menus[group.Path].controls = controls;
            }

            AssetDatabase.SaveAssets();
            expressionWizard.outputAsset = expressionMenu;
        }
    }
}