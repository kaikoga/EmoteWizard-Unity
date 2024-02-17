using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
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
                var target = serializedProperty.Lop(nameof(AnimatedEnable.target), LocalizationTool.Loc("AnimatedEnable::target"));
                var isEnable = serializedProperty.Lop(nameof(AnimatedEnable.isEnable), LocalizationTool.Loc("AnimatedEnable::isEnable"));

                EditorGUI.PropertyField(position.UISliceV(0), target.Property, target.GUIContent);
                EditorGUI.PropertyField(position.UISliceV(1), isEnable.Property, isEnable.GUIContent);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 1;
        }
    }
}