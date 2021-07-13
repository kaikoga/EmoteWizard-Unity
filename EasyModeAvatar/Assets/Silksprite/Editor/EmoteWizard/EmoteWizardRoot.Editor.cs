using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
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
            emoteWizardRoot = target as EmoteWizardRoot;
        }

        public override void OnInspectorGUI()
        {
            var serializedObj = serializedObject;
            using (new GUILayout.HorizontalScope())
            {
                emoteWizardRoot.generatedAssetRoot =
                    EditorGUILayout.TextField("Generated Assets Root", emoteWizardRoot.generatedAssetRoot);
                if (GUILayout.Button("Browse"))
                {
                    SelectFolder("Select Generated Assets Root", ref emoteWizardRoot.generatedAssetRoot);
                }
            }

            EditorGUILayout.PropertyField(serializedObj.FindProperty("generatedAssetPrefix"));
            CustomEditorGUILayout.PropertyFieldWithGenerate(serializedObj.FindProperty("emptyClip"), () => emoteWizardRoot.ProvideEmptyClip());

            EmoteWizardGUILayout.ConfigUIArea(() =>
            {
                EditorGUILayout.PropertyField(serializedObj.FindProperty("showTutorial"));
                EditorGUILayout.PropertyField(serializedObj.FindProperty("listDisplayMode"));
                EditorGUILayout.PropertyField(serializedObj.FindProperty("lowSpecMode"));
            });

            if (!emoteWizardRoot.GetWizard<SetupWizard>())
            {
                if (GUILayout.Button("Setup"))
                {
                    emoteWizardRoot.EnsureWizard<SetupWizard>();
                }
            }

            serializedObj.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "EmoteWizardの全体的な設定を行うコンポーネントです。\nEmoteWizardが生成したアセットはGenerated Assets Rootで指定したディレクトリの中に入ります。\nLow Spec Modeを無効にするとGameObjectが1つにまとまります（とても重い）\n\n基本的な使い方：\n1. Setup Wizardから必要なコンポーネントを生成する\n2. Avatar Wizardを設定する\n3. Expression Wizardからアクションメニューを設定する\n4. Parameter Wizardの設定値を確認する\n5. ハンドサインを差し替える場合はGesture Wizardから設定する\n6. FX Wizardから表情や着せ替えのアニメーションを設定する\n7. 必要に応じてAvatar Wizardから各アニメーションを編集する\n8. 全て終わったらAvatar WizardのUpdate Avatarを押す");
        }
    }
}