using Silksprite.EmoteWizardSupport.L10n;
using UnityEditor;
using UnityEngine;
using GUIContent = UnityEngine.GUIContent;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class EmoteWizardGUI
    {
        public static void Prop(Rect position, LocalizedProperty lop) => Prop(position, lop, lop.GUIContent);
        public static void Prop(Rect position, LocalizedProperty lop, GUIContent label) => EditorGUI.PropertyField(position, lop.Property, label);

        public static void PropAsLabel(Rect position, LocalizedProperty lop) => PropAsLabel(position, lop, lop.GUIContent);

        public static void PropAsLabel(Rect position, LocalizedProperty lop, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, lop.Property);
            if (lop.Property.hasMultipleDifferentValues)
            {
                EditorGUI.showMixedValue = true;
            }
            EditorGUI.LabelField(position, label, new GUIContent(lop.Property.stringValue));
            EditorGUI.showMixedValue = false;
            EditorGUI.EndProperty();
        }
    }
}