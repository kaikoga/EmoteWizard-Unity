using System.Collections.Generic;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
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
            position = position.UISliceV(2, -2);

            var fixedItemHeight = drawer.FixedPropertyHeight;
            var itemHeight = 0f;
            if (fixedItemHeight) itemHeight = drawer.GetPropertyHeight(default, GUIContent.none);
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                var itemLabel = new GUIContent(drawer.PagerItemName(item, i));
                if (!fixedItemHeight) itemHeight = drawer.GetPropertyHeight(item, itemLabel);
                drawer.OnGUI(position.SliceV(0, itemHeight), ref item, itemLabel);
                value[i] = item;
                itemHeight += EditorGUIUtility.standardVerticalSpacing;
                position = position.SliceV(itemHeight, -itemHeight);
            }

            return value;
        }
    }
}