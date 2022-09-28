using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUILayout
    {
        public static T ReferenceField<T>(string label, ref T value, params GUILayoutOption[] options) where T: Object
        {
            return value = (T) EditorGUILayout.ObjectField(label, value, typeof(T), true, options);
        }

        public static T ReferenceField<T>(GUIContent label, ref T value, params GUILayoutOption[] options) where T: Object
        {
            return value = (T) EditorGUILayout.ObjectField(label, value, typeof(T), true, options);
        }

        public static T AssetField<T>(string label, ref T value, params GUILayoutOption[] options) where T: Object
        {
            return value = (T) EditorGUILayout.ObjectField(label, value, typeof(T), false, options);
        }

        public static T AssetField<T>(GUIContent label, ref T value, params GUILayoutOption[] options) where T: Object
        {
            return value = (T) EditorGUILayout.ObjectField(label, value, typeof(T), false, options);
        }
    }
}