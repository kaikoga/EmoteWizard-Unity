using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.Tools;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(EmoteCondition))]
    public class EmoteConditionDrawer : PropertyDrawer
    {
        static readonly Dictionary<ParameterItemKind, EmoteConditionMode[]> Modes = new Dictionary<ParameterItemKind, EmoteConditionMode[]>
        {
            [ParameterItemKind.Auto] = new []
            {
                EmoteConditionMode.If,
                EmoteConditionMode.IfNot,
                EmoteConditionMode.Greater,
                EmoteConditionMode.Less,
                EmoteConditionMode.Equals,
                EmoteConditionMode.NotEqual,
            },
            [ParameterItemKind.Bool] = new []
            {
                EmoteConditionMode.If,
                EmoteConditionMode.IfNot,
            },
            [ParameterItemKind.Int] = new []
            {
                EmoteConditionMode.Greater,
                EmoteConditionMode.Less,
                EmoteConditionMode.Equals,
                EmoteConditionMode.NotEqual,
            },
            [ParameterItemKind.Float] = new []
            {
                EmoteConditionMode.Greater,
                EmoteConditionMode.Less,
            },
        };

        static readonly Dictionary<EmoteConditionMode[], string[]> DisplayModes = new Dictionary<EmoteConditionMode[], string[]>();

        static string[] GetDisplayModes(EmoteConditionMode[] modes)
        {
            if (!DisplayModes.TryGetValue(modes, out var displayModes))
            {
                // FIXME: Loch
                displayModes = modes.Select(mode => $"{mode}").ToArray();
                DisplayModes.Add(modes, displayModes);
            }
            return displayModes;
        }

        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var parameter = serializedProperty.Lop(nameof(EmoteCondition.parameter), Loc("EmoteCondition::parameter"));
            var kind = serializedProperty.Lop(nameof(EmoteCondition.kind), Loc("EmoteCondition::kind"));
            var mode = serializedProperty.Lop(nameof(EmoteCondition.mode), Loc("EmoteCondition::mode"));
            var threshold = serializedProperty.Lop(nameof(EmoteCondition.threshold), Loc("EmoteCondition::threshold"));

            var top = position.UISliceV(0);
            var bottom = position.UISliceV(1);
            using (new HideLabelsScope())
            {
                LEditorGUI.Prop(top.UISliceH(0.0f, 0.6f), parameter);
                LEditorGUI.Prop(top.UISliceH(0.6f, 0.4f), kind);
                ModePopup(bottom.UISliceH(0.1f, 0.4f), mode, (ParameterItemKind)kind.Property.intValue);
                LEditorGUI.Prop(bottom.UISliceH(0.5f, 0.5f), threshold);
            }
        }

        static void ModePopup(Rect position, LocalizedProperty mode, ParameterItemKind kind)
        {
            EditorGUI.BeginProperty(position, mode.Loc.GUIContent, mode.Property);
            if (!Modes.TryGetValue(kind, out var modes))
            {
                modes = Array.Empty<EmoteConditionMode>();
            }
            var modeValue = (EmoteConditionMode)mode.Property.intValue;
            var modeIndex = Array.IndexOf(modes, modeValue);
            var isInvalid = false;
            if (modeIndex == -1)
            {
                ArrayUtility.Add(ref modes, modeValue);
                modeIndex = modes.Length - 1;
                isInvalid = true;
            }
            using var _ = new InvalidValueScope(isInvalid);
            EditorGUI.BeginChangeCheck();
            modeIndex = EditorGUI.Popup(position, modeIndex, GetDisplayModes(modes).ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                mode.Property.intValue = (int)modes[modeIndex];
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return PropertyDrawerUITools.LineHeight(2);
        }
    }
}