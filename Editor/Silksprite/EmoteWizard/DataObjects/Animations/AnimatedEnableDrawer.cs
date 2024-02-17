using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
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
                var relativeRef = serializedProperty.Lop(nameof(AnimatedEnable.relativeRef), LocalizationTool.Loc("AnimatedEnable::relativeRef"));
                var isEnable = serializedProperty.Lop(nameof(AnimatedEnable.isEnable), LocalizationTool.Loc("AnimatedEnable::isEnable"));

                EmoteWizardGUI.Prop(position.UISliceV(0), relativeRef, label);
                EmoteWizardGUI.Prop(position.UISliceV(1), isEnable);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 1;
        }
    }
}