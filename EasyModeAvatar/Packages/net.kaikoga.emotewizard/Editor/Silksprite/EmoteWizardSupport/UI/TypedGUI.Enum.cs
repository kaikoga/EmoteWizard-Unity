using System;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUI
    {
        public static T EnumPopup<T>(Rect position, string label, ref T value)
            where T : Enum
        {
            return value = (T) EditorGUI.EnumPopup(position, label, value);
        }

        public static T EnumPopup<T>(Rect position, GUIContent label, ref T value)
            where T : Enum
        {
            return value = (T) EditorGUI.EnumPopup(position, label, value);
        }
    }
}