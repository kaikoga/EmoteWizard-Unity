using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUILayout
    {
        public static object UntypedField(ref object value, string label, params GUILayoutOption[] options) => UntypedField(ref value, new GUIContent(label), options);

        public static object UntypedField(ref object value, GUIContent label, params GUILayoutOption[] options)
        {
            if (value == null) value = TypeUtils.Duplicator()(default);
            var drawer = TypedDrawerRegistry.Drawer(value?.GetType());
            var o = value;
            var position = GUILayoutUtility.GetRect(0f, drawer.UntypedGetPropertyHeight(value, label));
            drawer.UntypedOnGUI(position, ref o, label);
            return value = o;
        }

        public static T TypedField<T>(ref T value, string label, params GUILayoutOption[] options) => TypedField(ref value, new GUIContent(label), options);

        public static T TypedField<T>(ref T value, GUIContent label, params GUILayoutOption[] options)
        {
            if (value == null) value = TypeUtils.Duplicator<T>()(default);
            var drawer = TypedDrawerRegistry<T>.Drawer;
            var position = GUILayoutUtility.GetRect(0f, drawer.GetPropertyHeight(value, label));
            drawer.OnGUI(position, ref value, label);
            return value;
        }
    }
}