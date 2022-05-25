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
            using (new ObjectChangeScope(expressionWizard))
            {
                var emoteWizardRoot = expressionWizard.EmoteWizardRoot;

                if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

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

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "Expression Menuの設定を一括で行い、アセットを出力します。\nここで入力した値は他のWizardに自動的に引き継がれます。\n項目名を半角スラッシュで区切るとサブメニューを作成できます。");
            }
        }

        void MigrateToDataSource()
        {
            var source = expressionWizard.AddChildComponent<ExpressionItemSource>();
            source.expressionItems = expressionWizard.legacyExpressionItems.ToList();
            expressionWizard.legacyExpressionItems.Clear();
            source.defaultPrefix = expressionWizard.defaultPrefix;
        }
    }
}