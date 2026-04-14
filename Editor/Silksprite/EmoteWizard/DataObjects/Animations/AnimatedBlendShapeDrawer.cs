using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.Loch.Extensions;
using Silksprite.Loch.IMGUI;
using Silksprite.Loch.Tools;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Animations
{
    [CustomPropertyDrawer(typeof(AnimatedBlendShape))]
    public class AnimatedBlendShapeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            using (new LabelWidthScope(100f))
            {
                var relativeRef = serializedProperty.Lop(nameof(AnimatedBlendShape.relativeRef),
                    LochTool.Loc("AnimatedBlendShape::relativeRef"));
                var blendShapeName = serializedProperty.Lop(
                    nameof(AnimatedBlendShape.blendShapeName),
                    LochTool.Loc("AnimatedBlendShape::blendShapeName"));
                var value = serializedProperty.Lop(nameof(AnimatedBlendShape.value),
                    LochTool.Loc("AnimatedBlendShape::value"));

                LEditorGUI.Prop(position.UISliceV(0), relativeRef);

                var skinnedMeshRenderer = (SkinnedMeshRenderer)relativeRef.Property.FindPropertyRelative(nameof(RelativeSkinnedMeshRendererRef.target)).objectReferenceValue;
                EmoteWizardGUI.PropAsBlendShapeName(position.UISliceV(1), blendShapeName, skinnedMeshRenderer);

                LEditorGUI.Prop(position.UISliceV(2), value);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2;
        }
    }
}