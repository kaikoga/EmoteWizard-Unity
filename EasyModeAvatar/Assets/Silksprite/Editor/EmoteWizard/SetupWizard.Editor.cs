using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(SetupWizard))]
    public class SetupWizardEditor : Editor
    {
        SetupWizard setupWizard;

        void OnEnable()
        {
            setupWizard = target as SetupWizard;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isSetupMode"), new GUIContent("Enable Setup Only UI"));
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Generate Wizards"))
            {
                setupWizard.EnsureComponent<AvatarWizard>();
                setupWizard.EnsureComponent<ExpressionWizard>();
                setupWizard.EnsureComponent<ParametersWizard>();
                setupWizard.EnsureComponent<GestureWizard>();
                setupWizard.EnsureComponent<FxWizard>();
            }
            if (GUILayout.Button("Complete setup and remove me"))
            {
                DestroyImmediate(setupWizard);
            }
            
            EmoteWizardGUILayout.Tutorial(setupWizard.EmoteWizardRoot, "EmoteWizardの初期セットアップと、破壊的な各設定のリセットを行います。\nセットアップ中に表示される各ボタンは既存の設定を一括で消去して上書きするため、注意して扱ってください。");
        }
    }
}