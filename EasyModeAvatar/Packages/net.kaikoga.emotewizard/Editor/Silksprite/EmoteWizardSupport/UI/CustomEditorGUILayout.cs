using System;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class CustomEditorGUILayout
    {
        public static void PropertyFieldWithGenerate(SerializedProperty serializedProperty, Func<UnityEngine.Object> generate)
        {
            PropertyFieldWithGenerateImpl(serializedProperty, null, generate);
        }
        public static void PropertyFieldWithGenerate(SerializedProperty serializedProperty, string label, Func<UnityEngine.Object> generate)
        {
            PropertyFieldWithGenerateImpl(serializedProperty, new GUIContent(label), generate);
        }

        static void PropertyFieldWithGenerateImpl(SerializedProperty serializedProperty, GUIContent label, Func<UnityEngine.Object> generate)
        {
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(serializedProperty, label);
                if (serializedProperty.objectReferenceValue == null && GUILayout.Button("Generate", GUILayout.Width(CustomEditorGUI.GenerateButtonWidth)))
                {
                    serializedProperty.objectReferenceValue = generate();
                }
            }
        }
    }
}