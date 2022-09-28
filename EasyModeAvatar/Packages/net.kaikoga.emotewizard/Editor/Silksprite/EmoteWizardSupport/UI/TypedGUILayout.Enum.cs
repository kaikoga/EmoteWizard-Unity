using System;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUILayout
    {
        public static T EnumPopup<T>(string label, ref T value, params GUILayoutOption[] options)
            where T : Enum
        {
            return value = (T) EditorGUILayout.EnumPopup(label, value, options);
        }

        public static T EnumPopup<T>(GUIContent label, ref T value, params GUILayoutOption[] options)
            where T : Enum
        {
            return value = (T) EditorGUILayout.EnumPopup(label, value, options);
        }
    }
}