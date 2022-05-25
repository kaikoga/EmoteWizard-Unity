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
                emoteWizardRoot.EnsureWizard<ActionWizard>();
            }

            EmoteWizardGUILayout.SetupOnlyUI(setupWizard, () =>
            {
                if (GUILayout.Button("Quick Setup 7 HandSigns"))
                {
                    QuickSetup(emoteWizardRoot);
                }

                if (GUILayout.Button("Quick Setup 14 HandSigns"))
                {
                    QuickSetup14(emoteWizardRoot);
                }
            });

            if (GUILayout.Button("Complete setup and remove me"))
            {
                DestroySelf(emoteWizardRoot);
                return;
            }
            
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, "EmoteWizardの初期セットアップと、破壊的な各設定のリセットを行います。\nセットアップ中に表示される各ボタンは既存の設定を一括で消去して上書きするため、注意して扱ってください。");
        }

        void DestroySelf(EmoteWizardRoot emoteWizardRoot)
        {
            if (setupWizard.gameObject != emoteWizardRoot.gameObject)
            {
                DestroyImmediate(setupWizard.gameObject, true);
            }
            else
            {
                DestroyImmediate(setupWizard, true);
            }
        }

        static void QuickSetup(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.EnsureWizard<AvatarWizard>();
            var expressionWizard = emoteWizardRoot.EnsureWizard<ExpressionWizard>();
            var parametersWizard = emoteWizardRoot.EnsureWizard<ParametersWizard>();
            var gestureWizard = emoteWizardRoot.EnsureWizard<GestureWizard>();
            var fxWizard = emoteWizardRoot.EnsureWizard<FxWizard>();
            var actionWizard = emoteWizardRoot.EnsureWizard<ActionWizard>();
            expressionWizard.FindOrCreateChildComponent<ExpressionItemSource>("Expression Sources").RepopulateDefaultExpressionItems();
            parametersWizard.RepopulateParameters();
            fxWizard.FindOrCreateChildComponent<FxEmoteSource>("FX Sources").RepopulateDefaultEmotes();
            fxWizard.FindOrCreateChildComponent<FxParameterEmoteSource>("FX Sources").RepopulateParameterEmotes(parametersWizard);
            gestureWizard.FindOrCreateChildComponent<GestureEmoteSource>("Gesture Sources").RepopulateDefaultEmotes();
            gestureWizard.FindOrCreateChildComponent<GestureParameterEmoteSource>("Gesture Sources").RepopulateParameterEmotes(parametersWizard);
            actionWizard.FindOrCreateChildComponent<ActionEmoteSource>("Action Sources").RepopulateDefaultActionEmotes();
            actionWizard.RepopulateDefaultAfkEmote();
        }

        static void QuickSetup14(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.EnsureWizard<AvatarWizard>();
            var expressionWizard = emoteWizardRoot.EnsureWizard<ExpressionWizard>();
            var parametersWizard = emoteWizardRoot.EnsureWizard<ParametersWizard>();
            var gestureWizard = emoteWizardRoot.EnsureWizard<GestureWizard>();
            var fxWizard = emoteWizardRoot.EnsureWizard<FxWizard>();
            var actionWizard = emoteWizardRoot.EnsureWizard<ActionWizard>();
            expressionWizard.FindOrCreateChildComponent<ExpressionItemSource>("Expression Sources").RepopulateDefaultExpressionItems();
            parametersWizard.RepopulateParameters();
            fxWizard.FindOrCreateChildComponent<FxEmoteSource>("FX Sources").RepopulateDefaultEmotes14();
            fxWizard.FindOrCreateChildComponent<FxParameterEmoteSource>("FX Sources").RepopulateParameterEmotes(parametersWizard);
            gestureWizard.FindOrCreateChildComponent<GestureEmoteSource>("Gesture Sources").RepopulateDefaultEmotes();
            gestureWizard.FindOrCreateChildComponent<GestureParameterEmoteSource>("Gesture Sources").RepopulateParameterEmotes(parametersWizard);
            actionWizard.FindOrCreateChildComponent<ActionEmoteSource>("Action Sources").RepopulateDefaultActionEmotes();
            actionWizard.RepopulateDefaultAfkEmote();
        }
    }
}