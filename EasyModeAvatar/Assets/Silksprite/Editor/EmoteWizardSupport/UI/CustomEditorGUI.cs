using System;
using System.Linq;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class CustomEditorGUI
    {
        public static void ColoredBox(Rect position, Color color)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(position, GUIContent.none);
            GUI.backgroundColor = backgroundColor;
        }

        public static void HorizontalListPropertyField(Rect position, SerializedProperty serializedProperty, GUIContent label = null)
        {
            if (label == null) label = new GUIContent(serializedProperty.displayName);

            var labelWidth = EditorGUIUtility.labelWidth;
            var sizeWidth = labelWidth * 0.25f;
            labelWidth *= 0.75f;
            GUI.Label(new Rect(position.x, position.y, labelWidth, EditorGUIUtility.singleLineHeight), label);
            using (new HideLabelsScope())
            {
                var arraySize = serializedProperty.arraySize;
                TypedGUI.DelayedIntField(new Rect(position.x + labelWidth, position.y, sizeWidth, EditorGUIUtility.singleLineHeight), "Size", ref arraySize);
                serializedProperty.arraySize = arraySize;
                position = position.Inset(labelWidth + sizeWidth, 0f, 0f, 0f);
                if (arraySize > 0)
                {
                    UnityEditor.EditorGUI.MultiPropertyField(position, Enumerable.Repeat(GUIContent.none, serializedProperty.arraySize).ToArray(), serializedProperty.GetArrayElementAtIndex(0));
                }
            }
        }

        public static void PropertyFieldWithGenerate(Rect position, SerializedProperty serializedProperty, Func<UnityEngine.Object> generate)
        {
            const float buttonWidth = 60;
            var fieldPosition = position;
            if (serializedProperty.objectReferenceValue == null) fieldPosition.width -= buttonWidth;

            UnityEditor.EditorGUI.PropertyField(fieldPosition, serializedProperty);
            if (serializedProperty.objectReferenceValue != null) return;
            var buttonPosition = new Rect(position.xMax - buttonWidth, position.y, buttonWidth, position.height);
            if (GUI.Button(buttonPosition, "Generate"))
            {
                serializedProperty.objectReferenceValue = generate();
            }
        }
    }
}