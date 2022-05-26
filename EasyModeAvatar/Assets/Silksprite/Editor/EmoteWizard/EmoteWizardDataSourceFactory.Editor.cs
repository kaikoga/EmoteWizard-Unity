using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardDataSourceFactory))]
    public class EmoteWizardDataSourceFactoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var sourceFactory = (EmoteWizardDataSourceFactory)target;
            using (new BoxLayoutScope())
            {
                GUILayout.Label("Add Source", new GUIStyle { fontStyle = FontStyle.Bold });
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Expression Item"))
                    {
                        sourceFactory.gameObject.AddComponent<ExpressionItemSource>();
                    }
                    if (GUILayout.Button("Parameter"))
                    {
                        sourceFactory.gameObject.AddComponent<ParameterSource>();
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Gesture", GUILayout.Width(60f));
                    if (GUILayout.Button("Emote"))
                    {
                        sourceFactory.gameObject.AddComponent<GestureEmoteSource>();
                    }
                    if (GUILayout.Button("Parameter Emote"))
                    {
                        sourceFactory.gameObject.AddComponent<GestureParameterEmoteSource>();
                    }
                    if (GUILayout.Button("Mixin"))
                    {
                        sourceFactory.gameObject.AddComponent<GestureAnimationMixinSource>();
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Fx", GUILayout.Width(60f));
                    if (GUILayout.Button("Emote"))
                    {
                        sourceFactory.gameObject.AddComponent<FxEmoteSource>();
                    }
                    if (GUILayout.Button("Parameter Emote"))
                    {
                        sourceFactory.gameObject.AddComponent<FxParameterEmoteSource>();
                    }
                    if (GUILayout.Button("Mixin"))
                    {
                        sourceFactory.gameObject.AddComponent<FxAnimationMixinSource>();
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Action Emote"))
                    {
                        sourceFactory.gameObject.AddComponent<ActionEmoteSource>();
                    }
                    if (GUILayout.Button("Afk Emote"))
                    {
                        sourceFactory.gameObject.AddComponent<AfkEmoteSource>();
                    }
                }
            }

            EmoteWizardGUILayout.Tutorial(sourceFactory.EmoteWizardRoot, Tutorial);
        }
        
        static string Tutorial =>
            string.Join("\n",
                "Emote Wizardに登録するデータの入力欄を生成します。",
                "GameObjectを非アクティブにした場合、データは無効化されます。",
                "",
                "Expression Item: Expression Menuのメニュー項目",
                "Parameter: 外部アセットが利用するExpression Parameter",
                "",
                "FX / Gesture共通項目:",
                "Emote: ハンドサインに基づくアニメーション",
                "Parameter Emote: パラメーターに基づくアニメーション",
                "Mixin: 常時再生したいアニメーションやブレンドツリー",
                "",
                "Action Emote: エモートモーション",
                "Afk Emote: カスタムAFKモーション");
    }
}