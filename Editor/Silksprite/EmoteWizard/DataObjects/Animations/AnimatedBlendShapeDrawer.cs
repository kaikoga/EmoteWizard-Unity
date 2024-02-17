using System;
using System.Linq;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
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
                var target = serializedProperty.Lop(nameof(AnimatedBlendShape.target),
                    LocalizationTool.Loc("AnimatedBlendShape::target"));
                var blendShapeName = serializedProperty.Lop(
                    nameof(AnimatedBlendShape.blendShapeName),
                    LocalizationTool.Loc("AnimatedBlendShape::blendShapeName"));
                var value = serializedProperty.Lop(nameof(AnimatedBlendShape.value),
                    LocalizationTool.Loc("AnimatedBlendShape::value"));

                EditorGUI.PropertyField(position.UISliceV(0), target.Property, target.GUIContent);

                var skinnedMeshRenderer = (SkinnedMeshRenderer)target.Property.objectReferenceValue;
                if (skinnedMeshRenderer && skinnedMeshRenderer.sharedMesh is Mesh sharedMesh)
                {
                    EditorGUI.BeginChangeCheck();
                    var options = Enumerable.Range(0, sharedMesh.blendShapeCount)
                        .Select(i => sharedMesh.GetBlendShapeName(i))
                        .ToArray();
                    var newBlendShapeNameValue = EditorGUI.Popup(
                        position.UISliceV(1),
                        blendShapeName.Loc.Tr,
                        Array.IndexOf(options, blendShapeName.Property.stringValue),
                        options
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        blendShapeName.Property.stringValue = options[newBlendShapeNameValue];
                    }
                }

                EditorGUI.PropertyField(position.UISliceV(2), value.Property, value.GUIContent);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2;
        }
    }
}