using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardDataSourceFactory))]
    public class EmoteWizardDataSourceFactoryEditor : Editor
    {
        EmoteWizardDataSourceFactory _sourceFactory;
        bool _templatesIsExpanded;

        DataSourceFactoryMode _mode;

        string _itemPath;
        bool _hasGroupName;
        string _groupName;
        bool _hasParameterName;
        string _parameterName;
        string _actionParameterName;
        int _actionIndex = -1;
        bool _hasExpressionItemSource;
        VRCExpressionsMenu _subMenu;

        void OnEnable()
        {
            _sourceFactory = (EmoteWizardDataSourceFactory)target;
        }

        static bool IsInvalidPathInput(string value) => string.IsNullOrWhiteSpace(value) || value.StartsWith("/") || value.EndsWith("/");
        static bool IsInvalidParameterInput(string value) => string.IsNullOrWhiteSpace(value) || value.Contains("/");

        string GuessActionFolderName()
        {
            var transform = _sourceFactory.transform;
            return transform.childCount > 0 ? transform.GetChild(transform.childCount - 1).name : "More Emotes";
        }

        int GuessActionIndex()
        {
            var snapshot = _sourceFactory.EmoteWizardRoot.EnsureWizard<ParametersWizard>().Snapshot();
            var newValue = 21;
            var usages = snapshot.ParameterItems.FirstOrDefault(v => v.Name == EmoteWizardConstants.Defaults.Params.ActionSelect)?.ReadUsages;
            if (usages != null)
            {
                while (usages.Any(usage => (int)usage.Value == newValue)) newValue++;
            }
            return newValue;
        }

        public override void OnInspectorGUI()
        {
            using (new BoxLayoutScope())
            {
                void ModeButton(DataSourceFactoryMode mode, string label, string desc, Action callback = null)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(new GUIContent(label, desc)))
                        {
                            _mode = mode;
                            callback?.Invoke();
                        }
                        GUILayout.Label(desc);
                    }
                }

                ModeButton(DataSourceFactoryMode.ExpressionMenu, "Expression Item Source", "Expression Menuのメニュー項目", () =>
                {
                    _itemPath = "";
                    _parameterName = "";
                });
                ModeButton(DataSourceFactoryMode.Parameter, "Parameter Source", "外部アセットが利用するパラメータ", () => _parameterName = "");
                ModeButton(DataSourceFactoryMode.Emote, "Emote Item Source", "アニメーションの発生条件", () => _itemPath = "");

                using (new EditorGUI.IndentLevelScope())
                {
                    _templatesIsExpanded = EditorGUILayout.Foldout(_templatesIsExpanded, "More Templates");
                    if (_templatesIsExpanded)
                    {
                        ModeButton(DataSourceFactoryMode.DressChange, "Dress Change", "着せ替えセット", () =>
                        {
                            _itemPath = "";
                            _parameterName = "";
                        });
                        ModeButton(DataSourceFactoryMode.CustomAction, "Custom Action", "エモートセット", () =>
                        {
                            _itemPath = $"{GuessActionFolderName()}/";
                            _actionParameterName = EmoteWizardConstants.Defaults.Params.ActionSelect;
                        });
                        ModeButton(DataSourceFactoryMode.SubMenu, "Sub Menu", "サブメニューセット", () => _itemPath = "");
                    }
                }
            }

            if (_mode != DataSourceFactoryMode.Default)
            {
                GUILayout.Label($"Add {_mode} Source", new GUIStyle { fontStyle = FontStyle.Bold });
            }

            var disableAddButton = false;

            void GroupName()
            {
                var isInvalidNameInput = IsInvalidPathInput(_groupName);
                disableAddButton |= isInvalidNameInput;
                _hasGroupName = EditorGUILayout.Toggle("Set Group Name", _hasGroupName);
                using (new EditorGUI.IndentLevelScope())
                using (new InvalidValueScope(isInvalidNameInput))
                {
                    _groupName = _hasGroupName ? EditorGUILayout.TextField("Group Name", _groupName) : _itemPath;
                }
            }

            void ItemPath()
            {
                var isInvalidNameInput = IsInvalidPathInput(_itemPath);
                disableAddButton |= isInvalidNameInput;
                using (new InvalidValueScope(isInvalidNameInput))
                {
                    _itemPath = EditorGUILayout.TextField("Item Path", _itemPath);
                }
            }

            void ParameterName(bool mandatory = false)
            {
                var isInvalidNameInput = IsInvalidParameterInput(_parameterName);
                disableAddButton |= isInvalidNameInput;
                _hasParameterName = mandatory || EditorGUILayout.Toggle("Set Parameter Name", _hasParameterName);
                using (new EditorGUI.IndentLevelScope(mandatory ? 0 : 1))
                using (new InvalidValueScope(isInvalidNameInput))
                {
                    _parameterName = _hasParameterName ? EditorGUILayout.TextField("Parameter Name", _parameterName) : _itemPath;
                }
            }

            void ActionParameterName()
            {
                var isInvalidNameInput = IsInvalidParameterInput(_actionParameterName);
                disableAddButton |= isInvalidNameInput;
                using (new InvalidValueScope(isInvalidNameInput))
                {
                    _actionParameterName = EditorGUILayout.TextField("Action Parameter Name", _actionParameterName);
                }
            }

            void ActionIndex()
            {
                if (_actionIndex < 0) _actionIndex = GuessActionIndex();
                _actionIndex = EditorGUILayout.IntField(_actionParameterName, _actionIndex);
            }

            void ExpressionsMenu()
            {
                disableAddButton |= !_subMenu;
                using (new InvalidValueScope(!_subMenu))
                {
                    _subMenu = EditorGUILayout.ObjectField("Sub Menu", _subMenu, typeof(VRCExpressionsMenu), false) as VRCExpressionsMenu;
                }
            }

            void HasExpressionItemSource()
            {
                _hasExpressionItemSource = EditorGUILayout.Toggle("Use Expression Item Source", _hasExpressionItemSource);
            }

            void AdvancedSettings(Action content)
            {
                using (new EditorGUI.IndentLevelScope())
                using (new BoxLayoutScope())
                {
                    GUILayout.Label($"Advanced Settings", new GUIStyle { fontStyle = FontStyle.Bold });
                    content();
                }
            }
            switch (_mode)
            {
                case DataSourceFactoryMode.Default:
                    break;
                case DataSourceFactoryMode.ExpressionMenu:
                    ItemPath();
                    AdvancedSettings(() =>
                    {
                        ParameterName();
                    });
                    using (new EditorGUI.DisabledScope(disableAddButton))
                    {
                        if (GUILayout.Button("Add")) GenerateExpressionMenu();
                    }
                    break;
                case DataSourceFactoryMode.Parameter:
                    ParameterName(true);
                    using (new EditorGUI.DisabledScope(disableAddButton))
                    {
                        if (GUILayout.Button("Add")) GenerateParameter();
                    }
                    break;
                case DataSourceFactoryMode.Emote:
                    ItemPath();
                    AdvancedSettings(() =>
                    {
                        GroupName();
                        ParameterName();
                        HasExpressionItemSource();
                    });
                    using (new EditorGUI.DisabledScope(disableAddButton))
                    {
                        if (GUILayout.Button("Add")) GenerateEmoteItem();
                    }
                    break;
                case DataSourceFactoryMode.DressChange:
                    ItemPath();
                    AdvancedSettings(() =>
                    {
                        GroupName();
                        ParameterName();
                        HasExpressionItemSource();
                    });
                    using (new EditorGUI.DisabledScope(disableAddButton))
                    {
                        if (GUILayout.Button("Add")) GenerateDressChangeTemplate();
                    }
                    break;
                case DataSourceFactoryMode.CustomAction:
                    ItemPath();
                    ActionIndex();
                    AdvancedSettings(() =>
                    {
                        ActionParameterName();
                        HasExpressionItemSource();
                    });
                    using (new EditorGUI.DisabledScope(disableAddButton))
                    {
                        if (GUILayout.Button("Add")) GenerateCustomActionTemplate();
                    }
                    break;
                case DataSourceFactoryMode.SubMenu:
                    ItemPath();
                    ExpressionsMenu();
                    using (new EditorGUI.DisabledScope(disableAddButton))
                    {
                        if (GUILayout.Button("Add")) GenerateSubMenuTemplate();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            EmoteWizardGUILayout.Tutorial(_sourceFactory.EmoteWizardRoot, Tutorial);
        }

        void GenerateExpressionMenu()
        {
            var expressionItemSource = _sourceFactory.AddChildComponent<ExpressionItemSource>(_itemPath);
            expressionItemSource.expressionItem.path = _itemPath;
            expressionItemSource.expressionItem.parameter = _parameterName;
        }

        void GenerateParameter()
        {
            var parameterSource = _sourceFactory.AddChildComponent<ParameterSource>(_parameterName);
            parameterSource.name = _parameterName;
        }

        void GenerateEmoteItem()
        {
            var child = _sourceFactory.AddChildGameObject(_itemPath);
            if (_hasExpressionItemSource)
            {
                child.AddComponent<ExpressionItemSource>().expressionItem = new ExpressionItem
                {
                    enabled = true,
                    icon = VrcSdkAssetLocator.ItemWand(),
                    path = _itemPath,
                    parameter = _parameterName,
                    itemKind = ExpressionItemKind.Toggle
                };
            }

            var emoteItemSource = child.AddComponent<EmoteItemSource>();
            emoteItemSource.trigger = new EmoteTrigger { name = _itemPath };
            emoteItemSource.hasExpressionItem = !_hasExpressionItemSource;
            emoteItemSource.expressionItemPath = _itemPath;
            emoteItemSource.expressionItemIcon = VrcSdkAssetLocator.ItemWand();
            child.AddComponent<EmoteSequenceSource>().sequence = new EmoteSequence { groupName = _groupName };
        }

        void GenerateDressChangeTemplate()
        {
            foreach (var value in Enumerable.Range(0, 2))
            {
                var childName = $"{_itemPath}/Item {value}";
                var child = _sourceFactory.AddChildGameObject(childName);
                if (_hasExpressionItemSource)
                {
                    child.AddComponent<ExpressionItemSource>().expressionItem = new ExpressionItem
                    {
                        enabled = true,
                        icon = VrcSdkAssetLocator.ItemWand(),
                        path = childName,
                        parameter = _parameterName,
                        value = value,
                        itemKind = ExpressionItemKind.Toggle
                    };
                }

                var emoteItemSource = child.AddComponent<EmoteItemSource>();
                emoteItemSource.trigger = new EmoteTrigger
                {
                    name = childName,
                    priority = 0,
                    conditions = new List<EmoteCondition>
                    {
                        new EmoteCondition
                        {
                            kind = ParameterItemKind.Int,
                            parameter = _parameterName,
                            mode = EmoteConditionMode.Equals,
                            threshold = value
                        }
                    }
                };
                emoteItemSource.hasExpressionItem = !_hasExpressionItemSource;
                emoteItemSource.expressionItemPath = _itemPath;
                emoteItemSource.expressionItemIcon = VrcSdkAssetLocator.ItemWand();
                child.AddComponent<EmoteSequenceSource>().sequence = new EmoteSequence
                {
                    layerKind = LayerKind.FX,
                    groupName = _groupName
                };
            }
        }

        void GenerateCustomActionTemplate()
        {
            var child = _sourceFactory.AddChildGameObject(_itemPath);

            if (_hasExpressionItemSource)
            {
                child.AddComponent<ExpressionItemSource>().expressionItem = new ExpressionItem
                {
                    enabled = true,
                    icon = VrcSdkAssetLocator.PersonDance(),
                    path = _itemPath,
                    parameter = _actionParameterName,
                    value = _actionIndex,
                    itemKind = ExpressionItemKind.Toggle
                };
            }

            var emoteItemSource = child.AddComponent<EmoteItemSource>();
            emoteItemSource.trigger = new EmoteTrigger
            {
                name = _itemPath,
                priority = 0,
                conditions = new List<EmoteCondition>
                {
                    new EmoteCondition
                    {
                        kind = ParameterItemKind.Int,
                        parameter = _actionParameterName,
                        mode = EmoteConditionMode.Equals,
                        threshold = _actionIndex
                    }
                }
            };
            emoteItemSource.hasExpressionItem = !_hasExpressionItemSource;
            emoteItemSource.expressionItemPath = _itemPath;
            emoteItemSource.expressionItemIcon = VrcSdkAssetLocator.PersonDance();
            child.AddComponent<EmoteSequenceSource>().sequence = new EmoteSequence
            {
                layerKind = LayerKind.Action,
                groupName = EmoteWizardConstants.Defaults.Groups.Action,
                hasLayerBlend = true,
                hasTrackingOverrides = true,
                trackingOverrides = new[]
                {
                    TrackingTarget.Head,
                    TrackingTarget.LeftHand,
                    TrackingTarget.RightHand,
                    TrackingTarget.Hip,
                    TrackingTarget.LeftFoot,
                    TrackingTarget.RightFoot,
                    TrackingTarget.LeftFingers,
                    TrackingTarget.RightFingers
                }.Select(t => new TrackingOverride { target = t }).ToList(),
                blendIn = 0.5f,
                blendOut = 0.25f
            };

            _actionIndex = -1;
        }

        void GenerateSubMenuTemplate()
        {
            _sourceFactory.AddChildComponent<ExpressionItemSource>(_itemPath).expressionItem = new ExpressionItem
            {
                enabled = true,
                icon = VrcSdkAssetLocator.ItemFolder(),
                path = _itemPath,
                value = 0,
                itemKind = ExpressionItemKind.SubMenu,
                subMenu = _subMenu
            };
        }

        static string Tutorial =>
            string.Join("\n",
                "Emote Wizardに登録するデータの入力欄を生成します。",
                "GameObjectを非アクティブにした場合、データは無効化されます。");
        
        enum DataSourceFactoryMode
        {
            Default,
            ExpressionMenu,
            Parameter,
            Emote,
            DressChange,
            CustomAction,
            SubMenu
        }
    }
}