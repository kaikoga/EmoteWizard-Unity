using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUILayout
    {
        public static string TextField(string label, ref string value, params GUILayoutOption[] options)
        {
            return value = EditorGUILayout.TextField(label, value, options);
        }

        public static string TextArea(ref string value, params GUILayoutOption[] options)
        {
            return value = EditorGUILayout.TextArea(value, options);
        }
    }
}