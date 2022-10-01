using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardDataSourceFactory))]
    public class EmoteWizardDataSourceFactoryEditor : Editor
    {
        bool _templatesIsExpanded;
        string _itemName;

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

                using (new EditorGUI.IndentLevelScope())
                {
                    _templatesIsExpanded = EditorGUILayout.Foldout(_templatesIsExpanded, "Templates");
                }
                if (_templatesIsExpanded)
                {
                    _itemName = EditorGUILayout.TextField("Name", _itemName);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(new GUIContent("Dress Change", "着せ替えセット")))
                        {
                            GenerateDressChangeTemplate(sourceFactory);
                        }
                        if (GUILayout.Button(new GUIContent("Action Emote", "エモートセット")))
                        {
                            GenerateActionEmoteTemplate(sourceFactory);
                        }
                        if (GUILayout.Button(new GUIContent("Sub Menu", "サブメニューセット")))
                        {
                            GenerateAssetTemplate(sourceFactory);
                        }
                    }
                }
            }

            EmoteWizardGUILayout.Tutorial(sourceFactory.EmoteWizardRoot, Tutorial);
        }

        void GenerateDressChangeTemplate(EmoteWizardDataSourceFactory sourceFactory)
        {
            var expressionItemSource = sourceFactory.gameObject.AddComponent<ExpressionItemSource>();
            foreach (var value in Enumerable.Range(1, 2))
            {
                expressionItemSource.expressionItems.Add(new ExpressionItem
                {
                    enabled = true,
                    icon = VrcSdkAssetLocator.ItemWand(),
                    path = $"{_itemName}/Item {value}",
                    parameter = _itemName,
                    value = value,
                    itemKind = ExpressionItemKind.Toggle
                });
            }

            var fxParameterEmoteSource = sourceFactory.gameObject.AddComponent<FxParameterEmoteSource>();
            var parametersWizard = sourceFactory.EmoteWizardRoot.GetWizard<ParametersWizard>();
            parametersWizard.RefreshParameters();
            fxParameterEmoteSource.GenerateSingleParameter(parametersWizard.AllParameterItems.First(p => p.name == _itemName));
        }

        void GenerateActionEmoteTemplate(EmoteWizardDataSourceFactory sourceFactory)
        {
            var expressionItemSource = sourceFactory.gameObject.AddComponent<ExpressionItemSource>();
            expressionItemSource.expressionItems.Add(new ExpressionItem
            {
                enabled = true,
                icon = VrcSdkAssetLocator.PersonDance(),
                path = $"More Emotes/{_itemName}",
                parameter = sourceFactory.EmoteWizardRoot.GetWizard<ActionWizard>().actionSelectParameter,
                value = 21,
                itemKind = ExpressionItemKind.Toggle
            });
            var actionEmoteSource = sourceFactory.gameObject.AddComponent<ActionEmoteSource>();
            actionEmoteSource.actionEmotes.Add(new ActionEmote
            {
                enabled = true,
                name = _itemName,
                emoteIndex = 21
            });
        }

        void GenerateAssetTemplate(EmoteWizardDataSourceFactory sourceFactory)
        {
            var expressionItemSource = sourceFactory.gameObject.AddComponent<ExpressionItemSource>();
            expressionItemSource.expressionItems.Add(new ExpressionItem
            {
                enabled = true,
                icon = VrcSdkAssetLocator.ItemFolder(),
                path = _itemName,
                value = 0,
                itemKind = ExpressionItemKind.SubMenu
            });
            var parameterSource = sourceFactory.gameObject.AddComponent<ParameterSource>();
            parameterSource.parameterItems.Add(new ParameterItem
            {
                enabled = true,
                name = _itemName,
                itemKind = ParameterItemKind.Int,
                saved = false,
                defaultValue = 0,
            });
        }

        static string Tutorial =>
            string.Join("\n",
                "Emote Wizardに登録するデータの入力欄を生成します。",
                "GameObjectを非アクティブにした場合、データは無効化されます。");
    }
}