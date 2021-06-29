using System;
using EmoteWizard.Base;
using EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class EditorUILayoutTools
    {
        public static void SetupOnlyUI(EmoteWizardBase emoteWizardBase, Action action)
        {
            if (!emoteWizardBase.IsSetupMode) return;
            
            using (new BoxLayoutScope(Color.magenta))
            {
                GUILayout.Label("Setup only zone");
                action();
            }
        }
        
        public static void ConfigUIArea(Action action)
        {
            using (new BoxLayoutScope(Color.yellow))
            {
                action();
            }
        }

        public static void OutputUIArea(Action action)
        {
            using (new BoxLayoutScope(Color.cyan))
            {
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