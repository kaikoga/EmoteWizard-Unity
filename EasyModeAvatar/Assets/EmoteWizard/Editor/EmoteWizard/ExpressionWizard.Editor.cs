using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects;
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
            base.OnInspectorGUI();

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

            var groups = expressionWizard.GroupExpressionItems();

            foreach (var group in groups)
            {
                var controls = group.Items.Select(item => item.ToControl()).ToList();
                if (group.Path == "")
                {
                    expressionMenu.controls = controls;
                    EditorUtility.SetDirty(expressionMenu);
                }
                else if (expressionWizard.buildAsSubAsset)
                {
                    var childMenu = CreateInstance<VRCExpressionsMenu>();
                    AssetDatabase.AddObjectToAsset(childMenu, rootItemPath);
                    childMenu.name = group.Path;
                    childMenu.controls = controls;
                }
                else
                {
                    var childMenu = CreateInstance<VRCExpressionsMenu>();
                    var childPath = $"{rootPath}{group.Path}.asset";
                    EnsureDirectory(childPath);
                    AssetDatabase.CreateAsset(childMenu, childPath);
                    childMenu.controls = controls;
                    EditorUtility.SetDirty(childMenu);
                }
            }
            AssetDatabase.SaveAssets();
            expressionWizard.outputAsset = expressionMenu;
        }
    }
}