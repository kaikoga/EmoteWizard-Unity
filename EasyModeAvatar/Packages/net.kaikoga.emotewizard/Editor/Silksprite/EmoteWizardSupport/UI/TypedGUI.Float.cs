using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUI
    {
        public static float FloatField(Rect position, string label, ref float value)
        {
            return value = EditorGUI.FloatField(position, label, value);
        }

        public static float FloatField(Rect position, GUIContent label, ref float value)
        {
            return value = EditorGUI.FloatField(position, label, value);
        }
    }
}