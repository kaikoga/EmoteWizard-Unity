using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUI
    {
        public static object UntypedField(Rect position, ref object value, string label) => UntypedField(position, ref value, new GUIContent(label));

        public static object UntypedField(Rect position, ref object value, GUIContent label)
        {
            var drawer = TypedDrawerRegistry.Drawer(value?.GetType());
            var o = value;
            drawer.UntypedOnGUI(position, ref o, label);
            return value = o;
        }

        public static T TypedField<T>(Rect position, ref T value, string label) => TypedField(position, ref value, new GUIContent(label));

        public static T TypedField<T>(Rect position, ref T value, GUIContent label)
        {
            var drawer = TypedDrawerRegistry<T>.Drawer;
            drawer.OnGUI(position, ref value, label);
            return value;
        }

        public static float UntypedGetPropertyHeight(object value, string label) => UntypedGetPropertyHeight(value, new GUIContent(label));

        public static float UntypedGetPropertyHeight(object value, GUIContent label)
        {
            var drawer = TypedDrawerRegistry.Drawer(value?.GetType());
            return drawer?.UntypedGetPropertyHeight(value, label) ?? EditorGUIUtility.singleLineHeight;
        }

        public static float GetPropertyHeight<T>(T value, string label) => GetPropertyHeight(value, new GUIContent(label));

        public static float GetPropertyHeight<T>(T value, GUIContent label)
        {
            var drawer = TypedDrawerRegistry<T>.Drawer;
            return drawer?.GetPropertyHeight(value, label) ?? EditorGUIUtility.singleLineHeight;
        }
    }
}