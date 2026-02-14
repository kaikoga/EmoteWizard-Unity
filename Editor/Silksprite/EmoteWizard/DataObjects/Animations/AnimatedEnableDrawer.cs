using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using Silksprite.Loch.Tools;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Animations
{
    [CustomPropertyDrawer(typeof(AnimatedEnable))]
    public class AnimatedEnableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            using (new LabelWidthScope(100f))
            {
                var relativeRef = serializedProperty.Lop(nameof(AnimatedEnable.relativeRef), LochTool.Loc("AnimatedEnable::relativeRef"));
                var isEnable = serializedProperty.Lop(nameof(AnimatedEnable.isEnable), LochTool.Loc("AnimatedEnable::isEnable"));

                LEditorGUI.Prop(position.UISliceV(0), relativeRef, label);
                LEditorGUI.Prop(position.UISliceV(1), isEnable);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 1;
        }
    }
}