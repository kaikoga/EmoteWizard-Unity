using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ExpressionItemSource))]
    public class ExpressionItemSourceEditor : EmoteWizardEditorBase<ExpressionItemSource>
    {
        protected override DetectedPlatform SupportedPlatforms => DetectedPlatform.VRChat;

        static readonly LocalizedContent[][] SubParameterLabels =
        {
            Array.Empty<LocalizedContent>(),
            new[]
            {
                Loc("ExpressionItem::subParameter::Rotation")
            },
            new[]
            {
                Loc("ExpressionItem::subParameter::Horizontal"),
                Loc("ExpressionItem::subParameter::Vertical")
            },
            Array.Empty<LocalizedContent>(),
            new[]
            {
                Loc("ExpressionItem::subParameter::Up"),
                Loc("ExpressionItem::subParameter::Right"),
                Loc("ExpressionItem::subParameter::Down"),
                Loc("ExpressionItem::subParameter::Left")
            }
        };

        LocalizedProperty _icon = null!;
        LocalizedProperty _path = null!;
        LocalizedProperty _parameter = null!;
        LocalizedProperty _value = null!;
        LocalizedProperty _itemKind = null!;
        LocalizedProperty _subParameters = null!;
        LocalizedProperty _labels = null!;
        LocalizedProperty _labelIcons = null!;
        LocalizedProperty _subMenu = null!;

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
            LEditorGUILayout.Prop(_icon);
            LEditorGUILayout.Prop(_path);
            using (new EditorGUILayout.HorizontalScope())
            {
                LEditorGUILayout.Prop(_parameter);
                using (new HideLabelsScope())
                {
                    GUILayout.Label("=", GUILayout.ExpandWidth(false));
                    LEditorGUILayout.Prop(_value);
                }
            }
            LEditorGUILayout.PropAsEnumPopup<ExpressionItemKind>(_itemKind);

            void DrawSubParameters(int subParametersCount, int labelsCount)
            {
                _subParameters.Property.arraySize = subParametersCount;
                if (subParametersCount > 0)
                {
                    LGUILayout.Heading(Loc("ExpressionItem::Puppet Parameters"));
                    using (new EditorGUI.IndentLevelScope())
                    {
                        for (var i = 0; i < subParametersCount; i++)
                        {
                            LGUILayout.Heading(SubParameterLabels[subParametersCount][i]);
                            using (new EditorGUI.IndentLevelScope())
                            {
                                LEditorGUILayout.Prop(_subParameters.GetArrayElementAtIndex(i, Loc("ExpressionItem::subParameters::subParameter")));
                            }
                        }
                    }
                }

                _labels.Property.arraySize = labelsCount;
                _labelIcons.Property.arraySize = labelsCount;
                if (labelsCount > 0)
                {
                    LGUILayout.Heading(Loc("ExpressionItem::Puppet Labels"));
                    using (new EditorGUI.IndentLevelScope())
                    {
                        for (var i = 0; i < labelsCount; i++)
                        {
                            LGUILayout.Heading(SubParameterLabels[labelsCount][i]);
                            using (new EditorGUI.IndentLevelScope())
                            {
                                LEditorGUILayout.Prop(_labels.GetArrayElementAtIndex(i, Loc("ExpressionItem::labels::label")));
                                LEditorGUILayout.Prop(_labelIcons.GetArrayElementAtIndex(i, Loc("ExpressionItem::labelIcons::labelIcon")));
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
                    using (new EditorGUI.DisabledScope(!SupportedPlatform.VRCSDK3_AVATARS))
                    {
                        LEditorGUILayout.Prop(_subMenu);
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
