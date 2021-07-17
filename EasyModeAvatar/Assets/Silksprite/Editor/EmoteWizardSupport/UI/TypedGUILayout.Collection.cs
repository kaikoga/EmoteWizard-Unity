using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUILayout
    {
        public static List<T> ListField<T>(string label, ref List<T> value, params GUILayoutOption[] options)
        {
            return ListField(new GUIContent(label), ref value, options);
        }
        
        public static List<T> ListField<T>(GUIContent label, ref List<T> value, params GUILayoutOption[] options)
        {
            var drawer = TypedDrawerRegistry<List<T>>.Drawer;
            var position = EditorGUILayout.GetControlRect(true, drawer.GetPropertyHeight(value, label));
            drawer.OnGUI(position, ref value, label);
            return value;
        }
    }
}