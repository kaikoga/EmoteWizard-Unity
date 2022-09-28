using System.Collections.Generic;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUI
    {
        public static List<T> ListField<T>(Rect position, string label, ref List<T> value)
            where T : new()
        {
            return ListField(position, new GUIContent(label), ref value);
        }
        
        public static List<T> ListField<T>(Rect position, GUIContent label, ref List<T> value)
            where T : new()
        {
            TypedDrawerRegistry<List<T>>.Drawer.OnGUI(position, ref value, label);
            return value;
        }
    }
}