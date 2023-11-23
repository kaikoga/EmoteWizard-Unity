using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardRoot))]
    public class EmoteWizardRootEditor : Editor
    {
        IEmoteWizardEnvironment _env;

        SerializedProperty _serializedGeneratedAssetRoot;
        SerializedProperty _serializedGeneratedAssetPrefix;
        SerializedProperty _serializedEmptyClip;
        SerializedProperty _serializedGenerateTrackingControlLayer;
        SerializedProperty _serializedShowTutorial;

        void OnEnable()
        {
            _env = ((EmoteWizardRoot)target).ToEnv();

            _serializedGeneratedAssetRoot = serializedObject.FindProperty(nameof(EmoteWizardRoot.generatedAssetRoot));
            _serializedGeneratedAssetPrefix = serializedObject.FindProperty(nameof(EmoteWizardRoot.generatedAssetPrefix));
            _serializedEmptyClip = serializedObject.FindProperty(nameof(EmoteWizardRoot.emptyClip));
            _serializedGenerateTrackingControlLayer = serializedObject.FindProperty(nameof(EmoteWizardRoot.generateTrackingControlLayer));
            _serializedShowTutorial = serializedObject.FindProperty(nameof(EmoteWizardRoot.showTutorial));
        }

        public override void OnInspectorGUI()
        {
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(_serializedGeneratedAssetRoot);
                if (GUILayout.Button("Browse"))
                {
                    SelectFolder("Select Generated Assets Root", _serializedGeneratedAssetRoot);
                }
            }

            EditorGUILayout.PropertyField(_serializedGeneratedAssetPrefix);
            CustomEditorGUILayout.PropertyFieldWithGenerate(_serializedEmptyClip, () => _env.ProvideEmptyClip());
            EditorGUILayout.PropertyField(_serializedGenerateTrackingControlLayer);

            EmoteWizardGUILayout.ConfigUIArea(() =>
            {
                EditorGUILayout.PropertyField(_serializedShowTutorial);
            });

            if (_env.GetContext<SetupContext>() != null)
            {
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Setup"))
                    {
                        _env.EnsureWizard<SetupWizard>();
                    }
                }
            }
            if (GUILayout.Button("Add Empty Data Source"))
            {
                _env.Transform.AddChildComponentAndSelect<EmoteWizardDataSourceFactory>("New Source");
            }
            if (GUILayout.Button("Disconnect Output Assets"))
            {
                _env.DisconnectAllOutputAssets();
            }
            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(_env, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "EmoteWizardの全体的な設定を行うコンポーネントです。",
                "EmoteWizardが生成したアセットはGenerated Assets Rootで指定したディレクトリの中に入ります。",
                "",
                "基本的な使い方：",
                "（上から順番に操作するのがお勧めです）",
                "1. Emote Wizard RootのSetupボタンを押す",
                "2. Setup WizardのGenerate Wizardsボタンを押して必要なコンポーネントを生成する",
                "3. Setup WizardのQuick Setupを押してプリセットを生成する",
                "4. Avatar Wizardを設定する",
                "5. FX Sourcesに表情や着せ替えの項目を設定する",
                "6. ハンドサインを差し替える場合はGesture Sourcesに設定する",
                "7. 必要に応じてAction SourcesからエモートとAFKモーションを編集する",
                "8. 外部アセットと連携する場合はExpression SourcesとParameter Sourcesを設定する",
                "9. Avatar WizardのUpdate Avatarを押してアバターを更新する",
                "10. Avatar WizardのEdit FXボタンなどを押してアニメーションを編集する");
    }
}