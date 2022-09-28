using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUILayout
    {
        public static bool Toggle(string label, ref bool value, params GUILayoutOption[] options)
        {
            return value = EditorGUILayout.Toggle(label, value, options);
        }

        public static bool Toggle(GUIContent label, ref bool value, params GUILayoutOption[] options)
        {
            return value = EditorGUILayout.Toggle(label, value, options);
        }

        public static bool ToggleLeft(string label, ref bool value, params GUILayoutOption[] options)
        {
            return value = EditorGUILayout.ToggleLeft(label, value, options);
        }

        public static bool ToggleLeft(GUIContent label, ref bool value, params GUILayoutOption[] options)
        {
            return value = EditorGUILayout.ToggleLeft(label, value, options);
        }

        public static bool Foldout(ref bool value, string label)
        {
            return value = EditorGUILayout.Foldout(value, label);
        }

        public static bool Foldout(object target, string label)
        {
            var value = IsExpandedTracker.GetIsExpanded(target);
            return EditorGUILayout.Foldout(value, label) == value ? value : IsExpandedTracker.SetIsExpanded(target, !value);
        }
    }
}