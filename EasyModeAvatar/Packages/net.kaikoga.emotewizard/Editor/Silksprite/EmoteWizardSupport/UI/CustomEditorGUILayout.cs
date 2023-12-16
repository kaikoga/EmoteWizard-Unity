using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class CustomEditorGUILayout
    {
        public static void PropertyFieldWithGenerate(SerializedProperty serializedProperty, Func<Object> generate)
        {
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(serializedProperty);
                if (serializedProperty.objectReferenceValue == null && GUILayout.Button("Generate", GUILayout.Width(CustomEditorGUI.GenerateButtonWidth)))
                {
                    serializedProperty.objectReferenceValue = generate();
                }
            }
        }
    }
}