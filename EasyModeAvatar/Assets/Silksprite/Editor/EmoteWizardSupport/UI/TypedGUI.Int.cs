using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUI
    {
        public static int IntField(Rect position, string label, ref int value)
        {
            return value = EditorGUI.IntField(position, label, value);
        }

        public static int DelayedIntField(Rect position, string label, ref int value)
        {
            return value = EditorGUI.DelayedIntField(position, label, value);
        }
    }
}