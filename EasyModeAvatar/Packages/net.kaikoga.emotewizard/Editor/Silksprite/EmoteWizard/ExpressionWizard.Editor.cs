using Silksprite.EmoteWizard.Extensions;
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

        static string Tutorial =>
            string.Join("\n",
                "Expression Menuを生成します。");

    }
}