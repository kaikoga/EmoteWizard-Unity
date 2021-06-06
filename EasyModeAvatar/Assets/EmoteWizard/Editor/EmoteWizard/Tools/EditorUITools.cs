using System;
using EmoteWizard.Base;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class EditorUITools
    {
        public static void SetupOnlyUI(EmoteWizardBase emoteWizardBase, Action action)
        {
            if (!emoteWizardBase.IsSetupMode) return;
            
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.magenta;
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                GUI.backgroundColor = backgroundColor;
                GUILayout.Label("Setup only zone");
                action();
            }
        }
        
        public static void OutputUIArea(Action action)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.cyan;
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                GUI.backgroundColor = backgroundColor;
                GUILayout.Label("Output zone");
                action();
            }
        }

        
        public static void PropertyFieldWithGenerate(SerializedProperty serializedProperty, Func<UnityEngine.Object> generate)
        {
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(serializedProperty);
                if (serializedProperty.objectReferenceValue == null && GUILayout.Button("Generate", GUILayout.Width(60f)))
                {
                    serializedProperty.objectReferenceValue = generate();
                }
            }
        }
    }
}