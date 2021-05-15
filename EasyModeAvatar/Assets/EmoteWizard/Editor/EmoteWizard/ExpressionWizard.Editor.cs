using System.Collections.Generic;
using EmoteWizard.Base;
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

            if (GUILayout.Button("Generate Expression Menu"))
            {
                BuildExpressionMenu();
            }
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
            var controls = new List<VRCExpressionsMenu.Control>
            {
                BuildDefaultEmoteMenuItem("Emote1", 1),
                BuildDefaultEmoteMenuItem("Emote2", 2),
                BuildDefaultEmoteMenuItem("Emote3", 3),
                BuildDefaultEmoteMenuItem("Emote4", 4),
                BuildDefaultEmoteMenuItem("Emote5", 5),
                BuildDefaultEmoteMenuItem("Emote6", 6),
                BuildDefaultEmoteMenuItem("Emote7", 7),
                BuildDefaultEmoteMenuItem("Emote8", 8)
            };
            expressionMenu.controls = controls;
            AssetDatabase.SaveAssets();
            expressionWizard.outputAsset = expressionMenu;
            EditorUtility.SetDirty(expressionMenu);
        }

        static VRCExpressionsMenu.Control BuildDefaultEmoteMenuItem(string name, int value)
        {
            return new VRCExpressionsMenu.Control
            {
                icon = VrcSdkAssetLocator.PersonDance(),
                labels = new VRCExpressionsMenu.Control.Label[] { },
                name = name,
                parameter = new VRCExpressionsMenu.Control.Parameter{name = "VRC_emote"},
                style = VRCExpressionsMenu.Control.Style.Style1,
                subMenu = null,
                subParameters = new VRCExpressionsMenu.Control.Parameter[] { },
                type = value == 8 ? VRCExpressionsMenu.Control.ControlType.Toggle : VRCExpressionsMenu.Control.ControlType.Button,
                value = value
            };
        }
    }
}