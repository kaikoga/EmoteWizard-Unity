using System;
using System.Linq;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
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
                    LocalizationTool.Loc("AnimatedBlendShape::relativeRef"));
                var blendShapeName = serializedProperty.Lop(
                    nameof(AnimatedBlendShape.blendShapeName),
                    LocalizationTool.Loc("AnimatedBlendShape::blendShapeName"));
                var value = serializedProperty.Lop(nameof(AnimatedBlendShape.value),
                    LocalizationTool.Loc("AnimatedBlendShape::value"));

                EmoteWizardGUI.Prop(position.UISliceV(0), relativeRef);

                var skinnedMeshRenderer = (SkinnedMeshRenderer)relativeRef.Property.FindPropertyRelative(nameof(RelativeSkinnedMeshRendererRef.target)).objectReferenceValue;
                if (skinnedMeshRenderer && skinnedMeshRenderer.sharedMesh is Mesh sharedMesh)
                {
                    EditorGUI.BeginChangeCheck();
                    var options = Enumerable.Range(0, sharedMesh.blendShapeCount)
                        .Select(i => sharedMesh.GetBlendShapeName(i))
                        .ToArray();
                    var newBlendShapeNameValue = EditorGUI.Popup(
                        position.UISliceV(2),
                        blendShapeName.Loc.Tr,
                        Array.IndexOf(options, blendShapeName.Property.stringValue),
                        options
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        blendShapeName.Property.stringValue = options[newBlendShapeNameValue];
                    }
                }
                else
                {
                    EmoteWizardGUI.PropAsLabel(position.UISliceV(2), blendShapeName);
                }

                EmoteWizardGUI.Prop(position.UISliceV(3), value);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 3;
        }
    }
}