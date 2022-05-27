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
                    if (GUILayout.Button(new GUIContent("Expression Item", "Expression Menuのメニュー項目")))
                    {
                        sourceFactory.gameObject.AddComponent<ExpressionItemSource>();
                    }
                    if (GUILayout.Button(new GUIContent("Parameter", "外部アセットが利用するExpression Parameter")))
                    {
                        sourceFactory.gameObject.AddComponent<ParameterSource>();
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Gesture", GUILayout.Width(60f));
                    if (GUILayout.Button(new GUIContent("Emote", "ハンドサインに基づくアニメーション(Gesture)")))
                    {
                        sourceFactory.gameObject.AddComponent<GestureEmoteSource>();
                    }
                    if (GUILayout.Button(new GUIContent("Parameter Emote", "パラメーターに基づくアニメーション(Gesture)")))
                    {
                        sourceFactory.gameObject.AddComponent<GestureParameterEmoteSource>();
                    }
                    if (GUILayout.Button(new GUIContent("Mixin", "常時再生したいアニメーションやブレンドツリー(Gesture)")))
                    {
                        sourceFactory.gameObject.AddComponent<GestureAnimationMixinSource>();
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("FX", GUILayout.Width(60f));
                    if (GUILayout.Button(new GUIContent("Emote", "ハンドサインに基づくアニメーション(FX)")))
                    {
                        sourceFactory.gameObject.AddComponent<FxEmoteSource>();
                    }
                    if (GUILayout.Button(new GUIContent("Parameter Emote", "パラメーターに基づくアニメーション(FX)")))
                    {
                        sourceFactory.gameObject.AddComponent<FxParameterEmoteSource>();
                    }
                    if (GUILayout.Button(new GUIContent("Mixin", "常時再生したいアニメーションやブレンドツリー(FX)")))
                    {
                        sourceFactory.gameObject.AddComponent<FxAnimationMixinSource>();
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent("Action Emote", "エモートモーション")))
                    {
                        sourceFactory.gameObject.AddComponent<ActionEmoteSource>();
                    }
                    if (GUILayout.Button(new GUIContent("Afk Emote", "カスタムAFKモーション")))
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
                "GameObjectを非アクティブにした場合、データは無効化されます。");
    }
}