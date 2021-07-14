using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUI
    {
        public static string TextField(Rect position, string label, ref string value)
        {
            return value = EditorGUI.TextField(position, label, value);
        }

        public static string TextArea(Rect position, string label, ref string value)
        {
            return value = EditorGUI.TextArea(position, label, value);
        }
    }
}