using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardRoot))]
    public class EmoteWizardRootEditor : Editor
    {
        EmoteWizardRoot emoteWizardRoot;

        void OnEnable()
        {
            emoteWizardRoot = (EmoteWizardRoot) target;
        }

        public override void OnInspectorGUI()
        {
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(emoteWizardRoot))
            {
                using (new GUILayout.HorizontalScope())
                {
                    TypedGUILayout.TextField("Generated Assets Root", ref emoteWizardRoot.generatedAssetRoot);
                    if (GUILayout.Button("Browse"))
                    {
                        SelectFolder("Select Generated Assets Root", ref emoteWizardRoot.generatedAssetRoot);
                    }
                }

                TypedGUILayout.TextField("Generated Asset Prefix", ref emoteWizardRoot.generatedAssetPrefix);
                CustomTypedGUILayout.AssetFieldWithGenerate("Empty Clip", ref emoteWizardRoot.emptyClip, () => emoteWizardRoot.ProvideEmptyClip());

                EmoteWizardGUILayout.ConfigUIArea(() =>
                {
                    TypedGUILayout.Toggle("Show Tutorial", ref emoteWizardRoot.showTutorial);
                    TypedGUILayout.Toggle("Copy Paste JSON", ref emoteWizardRoot.showCopyPasteJsonButtons);
                    TypedGUILayout.EnumPopup("List Display Mode", ref emoteWizardRoot.listDisplayMode);
                });

                if (!emoteWizardRoot.GetWizard<SetupWizard>())
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Setup"))
                        {
                            emoteWizardRoot.EnsureWizard<SetupWizard>();
                        }
                    }
                }
                if (GUILayout.Button("Disconnect Output Assets"))
                {
                    emoteWizardRoot.DisconnectAllOutputAssets();
                }
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "EmoteWizardの全体的な設定を行うコンポーネントです。",
                "EmoteWizardが生成したアセットはGenerated Assets Rootで指定したディレクトリの中に入ります。",
                "",
                "基本的な使い方：",
                "（上から順番に操作するのがお勧めです）",
                "1. Emote Wizard RootのSetupボタンを押す",
                "2. Setup WizardのQuick Setupから必要なコンポーネントを生成する",
                "3. Avatar Wizardを設定する",
                "4. Expression Wizardからアクションメニューを設定する",
                "5. Parameter Wizardの設定値を確認する",
                "6. ハンドサインを差し替える場合はGesture Wizardから設定する",
                "7. FX Wizardから表情や着せ替えのアニメーションを設定する",
                "8. 必要に応じてAction WizardからエモートとAFKモーションを編集する",
                "9. 全てのOutput zoneが埋まったらAvatar WizardのUpdate Avatarを押す");
    }
}