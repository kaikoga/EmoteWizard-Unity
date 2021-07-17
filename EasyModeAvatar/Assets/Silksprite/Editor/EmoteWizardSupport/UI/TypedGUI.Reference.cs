using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUI
    {
        public static T ReferenceField<T>(Rect position, string label, ref T value) where T: Object
        {
            return value = (T) EditorGUI.ObjectField(position, label, value, typeof(T), true);
        }

        public static T ReferenceField<T>(Rect position, GUIContent label, ref T value) where T: Object
        {
            return value = (T) EditorGUI.ObjectField(position, label, value, typeof(T), true);
        }

        public static T AssetField<T>(Rect position, string label, ref T value) where T: Object
        {
            return value = (T) EditorGUI.ObjectField(position, label, value, typeof(T), false);
        }
    }
}