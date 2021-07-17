using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
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
            setupWizard = (SetupWizard) target;
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(setupWizard))
            {
                TypedGUILayout.Toggle(new GUIContent("Enable Setup Only UI"), ref setupWizard.isSetupMode);
            }

            var emoteWizardRoot = setupWizard.EmoteWizardRoot;
            if (GUILayout.Button("Generate Wizards"))
            {
                emoteWizardRoot.EnsureWizard<AvatarWizard>();
                emoteWizardRoot.EnsureWizard<ExpressionWizard>();
                emoteWizardRoot.EnsureWizard<ParametersWizard>();
                emoteWizardRoot.EnsureWizard<GestureWizard>();
                emoteWizardRoot.EnsureWizard<FxWizard>();
            }
            if (GUILayout.Button("Complete setup and remove me"))
            {
                if (setupWizard.gameObject != emoteWizardRoot.gameObject)
                {
                    DestroyImmediate(setupWizard.gameObject);
                }
                else
                {
                    DestroyImmediate(setupWizard);
                }
                return;
            }
            
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "EmoteWizardの初期セットアップと、破壊的な各設定のリセットを行います。\nセットアップ中に表示される各ボタンは既存の設定を一括で消去して上書きするため、注意して扱ってください。");
        }
    }
}