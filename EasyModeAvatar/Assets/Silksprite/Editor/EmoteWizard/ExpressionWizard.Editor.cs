using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(ExpressionWizard))]
    public class ExpressionWizardEditor : Editor
    {
        ExpressionWizard expressionWizard;

        void OnEnable()
        {
            expressionWizard = (ExpressionWizard) target;
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = expressionWizard.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(expressionWizard))
            {
                TypedGUILayout.Toggle("Build As Sub Asset", ref expressionWizard.buildAsSubAsset);

                if (expressionWizard.HasLegacyData)
                {
                    EditorGUILayout.HelpBox("レガシーデータを検出しました。以下のボタンを押してエクスポートします。", MessageType.Warning);
                    if (GUILayout.Button("Migrate to Data Source"))
                    {
                        MigrateToDataSource();
                    }
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Expression Menu"))
                    {
                        expressionWizard.BuildOutputAsset();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref expressionWizard.outputAsset);
                });
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        void MigrateToDataSource()
        {
            var source = expressionWizard.AddChildComponent<ExpressionItemSource>();
            source.expressionItems = expressionWizard.legacyExpressionItems.ToList();
            expressionWizard.legacyExpressionItems.Clear();
            source.defaultPrefix = expressionWizard.defaultPrefix;
        }

        static string Tutorial =>
            string.Join("\n",
                "Expression Menuを生成します。");

    }
}