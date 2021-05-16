using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects;
using EmoteWizard.Tools;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
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

            if (GUILayout.Button("Repopulate Expression Items"))
            {
                RepopulateDefaultExpressionItems(expressionWizard);
            }
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
            var controls = expressionWizard.expressionItems.GroupBy(item => item.Folder)
                .First(group => group.Key == "")
                .Select(item => item.ToControl())
                .ToList();
            expressionMenu.controls = controls;
            AssetDatabase.SaveAssets();
            expressionWizard.outputAsset = expressionMenu;
            EditorUtility.SetDirty(expressionMenu);
        }
    }
}