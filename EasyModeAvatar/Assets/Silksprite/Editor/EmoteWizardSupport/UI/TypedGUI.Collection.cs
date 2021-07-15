using System.Collections.Generic;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUI
    {
        public static List<T> ListField<T>(Rect position, string label, ref List<T> value)
            where T : new()
        {
            return ListField(position, label, ref value, TypedDrawerRegistry.Drawer<T>());
        }
        
        public static List<T> ListField<T>(Rect position, string label, ref List<T> value, ITypedDrawer<T> drawer)
            where T : new()
        {
            if (!Foldout(position.UISliceV(0), value, label)) return value;

            var arraySize = value.Count;
            DelayedIntField(position.UISliceV(1), "Size", ref arraySize);
            ListUtils.ResizeAndPopulate(ref value, arraySize, v => new T());

            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                drawer.OnGUI(position.UISliceV(i + 2), ref item, new GUIContent(drawer.PagerItemName(item, i)));
                value[i] = item;
            }

            return value;
        }
    }
}