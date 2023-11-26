using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
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
        EmoteWizardRoot _root;

        SerializedProperty _serializedAvatarDescriptor;
        SerializedProperty _serializedProxyAnimator;
        SerializedProperty _serializedPersistGeneratedAssets;
        SerializedProperty _serializedGeneratedAssetRoot;
        SerializedProperty _serializedGeneratedAssetPrefix;
        SerializedProperty _serializedEmptyClip;
        SerializedProperty _serializedGenerateTrackingControlLayer;
        SerializedProperty _serializedShowTutorial;

        void OnEnable()
        {
            _root = (EmoteWizardRoot)target;

            _serializedAvatarDescriptor = serializedObject.FindProperty(nameof(EmoteWizardRoot.avatarDescriptor));
            _serializedProxyAnimator = serializedObject.FindProperty(nameof(EmoteWizardRoot.proxyAnimator));
            _serializedPersistGeneratedAssets = serializedObject.FindProperty(nameof(EmoteWizardRoot.persistGeneratedAssets));
            _serializedGeneratedAssetRoot = serializedObject.FindProperty(nameof(EmoteWizardRoot.generatedAssetRoot));
            _serializedGeneratedAssetPrefix = serializedObject.FindProperty(nameof(EmoteWizardRoot.generatedAssetPrefix));
            _serializedEmptyClip = serializedObject.FindProperty(nameof(EmoteWizardRoot.emptyClip));
            _serializedGenerateTrackingControlLayer = serializedObject.FindProperty(nameof(EmoteWizardRoot.generateTrackingControlLayer));
            _serializedShowTutorial = serializedObject.FindProperty(nameof(EmoteWizardRoot.showTutorial));
        }

        public override void OnInspectorGUI()
        {
            EmoteWizardGUILayout.OutputUIArea(() =>
            {
                var avatarDescriptorLabel = new GUIContent("Avatar Descriptor", "ここで指定したアバターの設定が上書きされます。");
                EditorGUILayout.PropertyField(_serializedAvatarDescriptor, avatarDescriptorLabel);

                var avatarDescriptor = _root.avatarDescriptor;
                if (avatarDescriptor == null)
                {
                    EditorGUILayout.HelpBox("VRCAvatarDescriptorを設定してください", MessageType.Error);
                }
                var proxyAnimatorLabel = new GUIContent("Proxy Animator", "アバターのアニメーションを編集する際に使用するAnimatorを別途選択できます。");
                EditorGUILayout.PropertyField(_serializedProxyAnimator, proxyAnimatorLabel);
                EditorGUILayout.PropertyField(_serializedPersistGeneratedAssets);

                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.PropertyField(_serializedGeneratedAssetRoot);
                    if (GUILayout.Button("Browse"))
                    {
                        SelectFolder("Select Generated Assets Root", _serializedGeneratedAssetRoot);
                    }
                }
                EditorGUILayout.PropertyField(_serializedGeneratedAssetPrefix);
                CustomEditorGUILayout.PropertyFieldWithGenerate(_serializedEmptyClip, () => _root.ToEnv().ProvideEmptyClip());
            });

            EditorGUILayout.PropertyField(_serializedGenerateTrackingControlLayer);

            EmoteWizardGUILayout.ConfigUIArea(() =>
            {
                EditorGUILayout.PropertyField(_serializedShowTutorial);
            });

            if (_root.ToEnv().GetContext<SetupContext>() != null)
            {
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Setup"))
                    {
                        _root.ToEnv().AddWizard<SetupWizard>();
                    }
                }
            }
            if (GUILayout.Button("Add Empty Data Source"))
            {
                _root.AddChildComponentAndSelect<EmoteWizardDataSourceFactory>("New Source");
            }
            if (GUILayout.Button("Disconnect Output Assets"))
            {
                _root.ToEnv().DisconnectAllOutputAssets();
            }
            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(_root.ToEnv(), Tutorial);
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