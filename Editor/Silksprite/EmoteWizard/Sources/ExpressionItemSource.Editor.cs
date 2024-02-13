using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ExpressionItemSource))]
    public class ExpressionItemSourceEditor : EmoteWizardEditorBase<ExpressionItemSource>
    {
        static readonly LocalizedContent[][] SubParameterLabels =
        {
            null,
            new[]
            {
                Loc("ExpressionItem::subParameter::Rotation")
            },
            new[]
            {
                Loc("ExpressionItem::subParameter::Horizontal"),
                Loc("ExpressionItem::subParameter::Vertical")
            },
            null,
            new[]
            {
                Loc("ExpressionItem::subParameter::Up"),
                Loc("ExpressionItem::subParameter::Right"),
                Loc("ExpressionItem::subParameter::Down"),
                Loc("ExpressionItem::subParameter::Left")
            }
        };

        LocalizedProperty _icon;
        LocalizedProperty _path;
        LocalizedProperty _parameter;
        LocalizedProperty _value;
        LocalizedProperty _itemKind;
        LocalizedProperty _subParameters;
        LocalizedProperty _labels;
        LocalizedProperty _labelIcons;
        LocalizedProperty _subMenu;

        void OnEnable()
        {
            var serializedItem = Lop(nameof(ExpressionItemSource.expressionItem), Loc("ExpressionItemSource::expressionItem"));

            _icon = serializedItem.Lop(nameof(ExpressionItem.icon), Loc("ExpressionItem::icon"));
            _path = serializedItem.Lop(nameof(ExpressionItem.path), Loc("ExpressionItem::path"));
            _parameter = serializedItem.Lop(nameof(ExpressionItem.parameter), Loc("ExpressionItem::parameter"));
            _value = serializedItem.Lop(nameof(ExpressionItem.value), Loc("ExpressionItem::value"));
            _itemKind = serializedItem.Lop(nameof(ExpressionItem.itemKind), Loc("ExpressionItem::itemKind"));
            _subParameters = serializedItem.Lop(nameof(ExpressionItem.subParameters), Loc("ExpressionItem::subParameters"));
            _labels = serializedItem.Lop(nameof(ExpressionItem.labels), Loc("ExpressionItem::labels"));
            _labelIcons = serializedItem.Lop(nameof(ExpressionItem.labelIcons), Loc("ExpressionItem::labelIcons"));
            _subMenu = serializedItem.Lop(nameof(ExpressionItem.subMenu), Loc("ExpressionItem::subMenu"));
        }

        protected override void OnInnerInspectorGUI()
        {
            EmoteWizardGUILayout.Prop(_icon);
            EmoteWizardGUILayout.Prop(_path);
            using (new EditorGUILayout.HorizontalScope())
            {
                EmoteWizardGUILayout.Prop(_parameter);
                using (new HideLabelsScope())
                {
                    GUILayout.Label("=", GUILayout.ExpandWidth(false));
                    EmoteWizardGUILayout.Prop(_value);
                }
            }
            EmoteWizardGUILayout.Prop(_itemKind);

            void DrawSubParameters(int subParametersCount, int labelsCount)
            {
                _subParameters.Property.arraySize = subParametersCount;
                if (subParametersCount > 0)
                {
                    EmoteWizardGUILayout.Header(Loc("ExpressionItem::Puppet Parameters"));
                    using (new EditorGUI.IndentLevelScope())
                    {
                        for (var i = 0; i < subParametersCount; i++)
                        {
                            EmoteWizardGUILayout.Header(SubParameterLabels[subParametersCount][i]);
                            using (new EditorGUI.IndentLevelScope())
                            {
                                EmoteWizardGUILayout.Prop(_subParameters.GetArrayElementAtIndex(i, Loc("ExpressionItem::subParameters::subParameter")));
                            }
                        }
                    }
                }

                _labels.Property.arraySize = labelsCount;
                _labelIcons.Property.arraySize = labelsCount;
                if (labelsCount > 0)
                {
                    EmoteWizardGUILayout.Header(Loc("ExpressionItem::Puppet Labels"));
                    using (new EditorGUI.IndentLevelScope())
                    {
                        for (var i = 0; i < labelsCount; i++)
                        {
                            EmoteWizardGUILayout.Header(SubParameterLabels[labelsCount][i]);
                            using (new EditorGUI.IndentLevelScope())
                            {
                                EmoteWizardGUILayout.Prop(_labels.GetArrayElementAtIndex(i, Loc("ExpressionItem::labels::label")));
                                EmoteWizardGUILayout.Prop(_labelIcons.GetArrayElementAtIndex(i, Loc("ExpressionItem::labelIcons::labelIcon")));
                            }
                        }
                    }
                }
            }

            if (_itemKind.Property.hasMultipleDifferentValues) return;
            switch ((ExpressionItemKind)_itemKind.Property.intValue)
            {
                case ExpressionItemKind.Button:
                case ExpressionItemKind.Toggle:
                    break;
                case ExpressionItemKind.SubMenu:
                    using (new EditorGUI.DisabledScope(!EmoteWizardConstants.SupportedPlatforms.VRCSDK3_AVATARS))
                    {
                        EmoteWizardGUILayout.Prop(_subMenu);
                    }
                    break;
                case ExpressionItemKind.TwoAxisPuppet:
                    DrawSubParameters(2, 4);
                    break;
                case ExpressionItemKind.FourAxisPuppet:
                    DrawSubParameters(4, 4);
                    break;
                case ExpressionItemKind.RadialPuppet:
                    DrawSubParameters(1, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
