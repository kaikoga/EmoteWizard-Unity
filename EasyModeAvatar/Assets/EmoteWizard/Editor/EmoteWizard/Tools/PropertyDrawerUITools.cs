using System;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class PropertyDrawerUITools
    {
        public static float LineHeight(float line, float dSpacing = -1f)
        {
            return EditorGUIUtility.singleLineHeight * line +
                   EditorGUIUtility.standardVerticalSpacing * (line + dSpacing);
        }

        public static float LineTop(float line, float dSpacing = 0f)
        {
            return EditorGUIUtility.singleLineHeight * line +
                   EditorGUIUtility.standardVerticalSpacing * (line + dSpacing);
        }

        public static float BoxHeight(float height)
        {
            return height + GUI.skin.box.padding.vertical * 2;
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