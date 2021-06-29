using System;
using EmoteWizard.Base;
using EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.UI
{
    public static class EmoteWizardGUI
    {
        public static void ColoredBox(Rect position, Color color)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(position, GUIContent.none);
            GUI.backgroundColor = backgroundColor;
        }

        public static void PropertyFieldWithGenerate(Rect position, SerializedProperty serializedProperty, Func<UnityEngine.Object> generate)
        {
            const float buttonWidth = 60;
            var fieldPosition = position;
            if (serializedProperty.objectReferenceValue == null) fieldPosition.width -= buttonWidth;

            EditorGUI.PropertyField(fieldPosition, serializedProperty);
            if (serializedProperty.objectReferenceValue != null) return;
            var buttonPosition = new Rect(position.xMax - buttonWidth, position.y, buttonWidth, position.height);
            if (GUI.Button(buttonPosition, "Generate"))
            {
                serializedProperty.objectReferenceValue = generate();
            }
        }
    }
}