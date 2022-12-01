using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
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
                    if (GUILayout.Button(new GUIContent("Expression Item Source", "Expression Menuのメニュー項目")))
                    {
                        sourceFactory.gameObject.AddComponent<ExpressionItemSource>();
                    }
                    GUILayout.Label("Expression Menuのメニュー項目");
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent("Parameter Source", "外部アセットが利用するパラメータ")))
                    {
                        sourceFactory.gameObject.AddComponent<ParameterSource>();
                    }
                    GUILayout.Label("外部アセットが利用するパラメータ");
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent("Emote Item Source", "アニメーションの発生条件")))
                    {
                        sourceFactory.gameObject.AddComponent<EmoteItemSource>();
                    }
                    GUILayout.Label("アニメーションの発生条件");
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent("Emote Sequence Source", "アニメーションの内容")))
                    {
                        sourceFactory.gameObject.AddComponent<EmoteSequenceSource>();
                    }
                    GUILayout.Label("アニメーションの内容");
                }

                using (new EditorGUI.IndentLevelScope())
                {
                    _templatesIsExpanded = EditorGUILayout.Foldout(_templatesIsExpanded, "Templates");
                }
                if (_templatesIsExpanded)
                {
                    _itemName = EditorGUILayout.TextField("Name", _itemName);

                    using (new EditorGUI.DisabledScope(string.IsNullOrWhiteSpace(_itemName)))
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
            foreach (var value in Enumerable.Range(1, 2))
            {
                var child = sourceFactory.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>($"Item {value}");
                child.gameObject.AddComponent<ExpressionItemSource>().expressionItem = new ExpressionItem
                {
                    enabled = true,
                    icon = VrcSdkAssetLocator.ItemWand(),
                    path = $"{_itemName}/Item {value}",
                    parameter = _itemName,
                    value = value,
                    itemKind = ExpressionItemKind.Toggle
                };
                child.gameObject.AddComponent<EmoteItemSource>().trigger = new EmoteTrigger
                {
                    name = $"{_itemName}/Item {value}",
                    layerKind = LayerKind.FX,
                    groupName = _itemName,
                    priority = 0,
                    conditions = new List<EmoteCondition>
                    {
                        new EmoteCondition
                        {
                            kind = ParameterItemKind.Int,
                            parameter = _itemName,
                            mode = EmoteConditionMode.Equals,
                            threshold = value
                        }
                    }
                };
                child.gameObject.AddComponent<EmoteSequenceSource>();
            }
        }

        void GenerateActionEmoteTemplate(EmoteWizardDataSourceFactory sourceFactory)
        {
            const int value = 21;

            var child = sourceFactory.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(_itemName);

            child.gameObject.AddComponent<ExpressionItemSource>().expressionItem = new ExpressionItem
            {
                enabled = true,
                icon = VrcSdkAssetLocator.PersonDance(),
                path = $"More Emotes/{_itemName}",
                parameter = EmoteWizardConstants.Defaults.Params.ActionSelect,
                value = value,
                itemKind = ExpressionItemKind.Toggle
            };
            child.gameObject.AddComponent<EmoteItemSource>().trigger = new EmoteTrigger
            {
                name = $"More Emotes/{_itemName}",
                layerKind = LayerKind.Action,
                groupName = "Action",
                priority = 0,
                conditions = new List<EmoteCondition>
                {
                    new EmoteCondition
                    {
                        kind = ParameterItemKind.Int,
                        parameter = EmoteWizardConstants.Defaults.Params.ActionSelect,
                        mode = EmoteConditionMode.Equals,
                        threshold = value
                    }
                }
            };
            child.gameObject.AddComponent<EmoteSequenceSource>();
        }

        void GenerateAssetTemplate(EmoteWizardDataSourceFactory sourceFactory)
        {
            var child = sourceFactory.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>(_itemName);

            child.gameObject.AddComponent<ExpressionItemSource>().expressionItem = new ExpressionItem
            {
                enabled = true,
                icon = VrcSdkAssetLocator.ItemFolder(),
                path = _itemName,
                value = 0,
                itemKind = ExpressionItemKind.SubMenu
            };
        }

        static string Tutorial =>
            string.Join("\n",
                "Emote Wizardに登録するデータの入力欄を生成します。",
                "GameObjectを非アクティブにした場合、データは無効化されます。");
    }
}