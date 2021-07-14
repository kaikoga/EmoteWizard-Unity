using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUI
    {
        public static bool Toggle(Rect position, string label, ref bool value)
        {
            return value = EditorGUI.Toggle(position, label, value);
        }

        public static bool ToggleLeft(Rect position, string label, ref bool value)
        {
            return value = EditorGUI.ToggleLeft(position, label, value);
        }

        public static bool Foldout(Rect position, ref bool value, string label)
        {
            return value = EditorGUI.Foldout(position, value, label);
        }
    }
}