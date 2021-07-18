using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
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
        protected static string Tutorial =>
            string.Join("\n",
                "EmoteWizardの全体的な設定を行うコンポーネントです。",
                "EmoteWizardが生成したアセットはGenerated Assets Rootで指定したディレクトリの中に入ります。",
                "Low Spec Modeを無効にするとGameObjectが1つにまとまります（とても重い）",
                "",
                "基本的な使い方：",
                "1. Setup Wizardから必要なコンポーネントを生成する",
                "2. 各WizardのSetup only zoneのボタンを全部押して入力欄を生成する",
                "3. Avatar Wizardを設定する",
                "4. Expression Wizardからアクションメニューを設定する",
                "5. Parameter Wizardの設定値を確認する",
                "6. ハンドサインを差し替える場合はGesture Wizardから設定する",
                "7. FX Wizardから表情や着せ替えのアニメーションを設定する",
                "8. 必要に応じてAvatar Wizardから各アニメーションを編集する",
                "9. 全て終わったらAvatar WizardのUpdate Avatarを押す");

        EmoteWizardRoot emoteWizardRoot;

        void OnEnable()
        {
            emoteWizardRoot = (EmoteWizardRoot) target;
        }

        public override void OnInspectorGUI()
        {
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
                    TypedGUILayout.EnumPopup("List Display Mode", ref emoteWizardRoot.listDisplayMode);
                    TypedGUILayout.Toggle("Low Spec UI", ref emoteWizardRoot.lowSpecUI);
                });

                if (!emoteWizardRoot.GetWizard<SetupWizard>())
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Setup Low Spec UI"))
                        {
                            emoteWizardRoot.lowSpecUI = true;
                            emoteWizardRoot.EnsureWizard<SetupWizard>();
                        }
                        if (GUILayout.Button("Setup High Spec UI"))
                        {
                            emoteWizardRoot.lowSpecUI = false;
                            emoteWizardRoot.EnsureWizard<SetupWizard>();
                        }
                    }
                }

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
            }
        }
    }
}