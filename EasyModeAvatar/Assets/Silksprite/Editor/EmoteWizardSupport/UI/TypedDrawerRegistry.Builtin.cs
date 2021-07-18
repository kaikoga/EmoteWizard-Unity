using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Tools;
using Silksprite.EmoteWizardSupport.UI.Base;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedDrawerRegistry
    {
        class InvalidDrawer : TypedDrawerBase<object>, IInvalidTypedDrawer
        {
            public override void OnGUI(Rect position, ref object property, GUIContent label)
            {
                EditorGUI.LabelField(position, label, new GUIContent($"???{property?.GetType().Name ?? "Null"} Drawer???"));
            }
        }

        [UsedImplicitly]
        class IntDrawer : TypedDrawerBase<int>
        {
            public override void OnGUI(Rect position, ref int property, GUIContent label)
            {
                TypedGUI.IntField(position, label, ref property);
            }
        }

        [UsedImplicitly]
        class FloatDrawer : TypedDrawerBase<float>
        {
            public override void OnGUI(Rect position, ref float property, GUIContent label)
            {
                TypedGUI.FloatField(position, label, ref property);
            }
        }

        [UsedImplicitly]
        class BoolDrawer : TypedDrawerBase<bool>
        {
            public override void OnGUI(Rect position, ref bool property, GUIContent label)
            {
                TypedGUI.Toggle(position, label, ref property);
            }
        }

        [UsedImplicitly]
        class StringDrawer : TypedDrawerBase<string>
        {
            public override void OnGUI(Rect position, ref string property, GUIContent label)
            {
                TypedGUI.TextField(position, label, ref property);
            }
        }

        [UsedImplicitly]
        class EnumDrawer : TypedDrawerBase<Enum>
        {
            public override void OnGUI(Rect position, ref Enum property, GUIContent label)
            {
                TypedGUI.EnumPopup(position, label, ref property);
            }
        }

        [UsedImplicitly]
        class ObjectDrawer<T> : TypedDrawerBase<T>
        where T : Object
        {
            public override void OnGUI(Rect position, ref T property, GUIContent label)
            {
                TypedGUI.ReferenceField(position, label, ref property);
            }
        }

        abstract class CollectionDrawerBase<T, TList, TDrawer> : TypedDrawerBase<TList>
            where TList : class, IList
        {
            protected abstract void ResizeAndPopulate(ref TList property, int arraySize);
            protected abstract TDrawer SharedDrawer(TList property);
            protected abstract void ItemDrawer(ref TDrawer drawer, T property);

            protected abstract T Item(TList property, int index);
            protected abstract void Item(TList property, int index, T value);
            protected abstract float DrawItem(Rect position, TDrawer drawer, ref T property, int index);

            protected abstract bool FixedPropertyHeight(TDrawer drawer);
            protected abstract float GetPropertyHeight(TDrawer drawer, T property);

            public sealed override void OnGUI(Rect position, ref TList property, GUIContent label)
            {
                if (property == null) ResizeAndPopulate(ref property, 0);
                if (!TypedGUI.Foldout(position.UISliceV(0), property, label)) return;

                const int arraySizeMax = 100;
                var arraySize = property.Count;
                TypedGUI.DelayedIntField(position.UISliceV(1), "Size", ref arraySize);
                if (arraySize > arraySizeMax) arraySize = arraySizeMax;
                ResizeAndPopulate(ref property, arraySize);
                position = position.UISliceV(2, -2);
                var drawer = SharedDrawer(property);
                for (var i = 0; i < property.Count; i++)
                {
                    var item = Item(property, i);
                    ItemDrawer(ref drawer, item);
                    var height = DrawItem(position, drawer, ref item, i) + EditorGUIUtility.standardVerticalSpacing;
                    Item(property, i, item);
                    position = position.SliceV(height, -height);
                }
            }

            public sealed override float GetPropertyHeight(TList property, GUIContent label)
            {
                if (!IsExpandedTracker.GetIsExpanded(property)) return EditorGUIUtility.singleLineHeight;
                var drawer = SharedDrawer(property);
                var height = PropertyDrawerUITools.LineHeight(2f);

                if (FixedPropertyHeight(drawer))
                {
                    height += property.Count * (GetPropertyHeight(drawer, default) + EditorGUIUtility.standardVerticalSpacing);
                }
                else
                {
                    for (var i = 0; i < property.Count; i++)
                    {
                        height += GetPropertyHeight(drawer, Item(property, i)) + EditorGUIUtility.standardVerticalSpacing;
                    }
                }

                return height;
            }
        }

        [UsedImplicitly]
        class ListDrawer : CollectionDrawerBase<object, IList, UntypedDrawer>
        {
            protected override void ResizeAndPopulate(ref IList property, int arraySize) => ListUtils.ResizeAndPopulate(ref property, arraySize);
            protected override UntypedDrawer SharedDrawer(IList property) => null;
            protected override void ItemDrawer(ref UntypedDrawer drawer, object item) => drawer = Drawer(item?.GetType());

            protected override object Item(IList property, int index) => property[index];
            protected override void Item(IList property, int index, object value) => property[index] = value;

            protected override float DrawItem(Rect position, UntypedDrawer drawer, ref object item, int index)
            {
                if (item == null) return EditorGUIUtility.singleLineHeight;
                var itemLabel = drawer.UntypedPagerItemName(item, index);
                var itemHeight = drawer.UntypedGetPropertyHeight(item, new GUIContent(itemLabel));
                TypedGUI.UntypedField(position.SliceV(0, itemHeight), ref item, itemLabel);
                return itemHeight;
            }

            protected override bool FixedPropertyHeight(UntypedDrawer drawer) => false;
            protected override float GetPropertyHeight(UntypedDrawer drawer, object item)
            {
                if (item == null) return EditorGUIUtility.singleLineHeight;
                return drawer.UntypedGetPropertyHeight(item, GUIContent.none);
            }
        }

        [UsedImplicitly]
        class ArrayDrawer<T> : CollectionDrawerBase<T, T[], ITypedDrawer<T>>
        {
            protected override void ResizeAndPopulate(ref T[] property, int arraySize) => ArrayUtils.ResizeAndPopulate(ref property, arraySize);
            protected override ITypedDrawer<T> SharedDrawer(T[] property) => TypedDrawerRegistry<T>.Drawer;
            protected override void ItemDrawer(ref ITypedDrawer<T> drawer, T item) { }
            protected override T Item(T[] property, int index) => property[index];
            protected override void Item(T[] property, int index, T value) => property[index] = value;

            protected override float DrawItem(Rect position, ITypedDrawer<T> drawer, ref T item, int index)
            {
                if (item == null) return EditorGUIUtility.singleLineHeight;
                var itemLabel = drawer.PagerItemName(item, index);
                var itemHeight = drawer.GetPropertyHeight(item, new GUIContent(itemLabel));
                TypedGUI.TypedField(position.SliceV(0, itemHeight), ref item, itemLabel);
                return itemHeight;
            }

            protected override bool FixedPropertyHeight(ITypedDrawer<T> drawer) => drawer.FixedPropertyHeight;
            protected override float GetPropertyHeight(ITypedDrawer<T> drawer, T item)
            {
                if (item == null) return EditorGUIUtility.singleLineHeight;
                return drawer.GetPropertyHeight(item, GUIContent.none);
            }
        }
        
        [UsedImplicitly]
        class ListDrawer<T> : CollectionDrawerBase<T, List<T>, ITypedDrawer<T>>
        {
            protected override void ResizeAndPopulate(ref List<T> property, int arraySize) => ListUtils.ResizeAndPopulate(ref property, arraySize);
            protected override ITypedDrawer<T> SharedDrawer(List<T> property) => TypedDrawerRegistry<T>.Drawer;
            protected override void ItemDrawer(ref ITypedDrawer<T> drawer, T item) { }

            protected override T Item(List<T> property, int index) => property[index];
            protected override void Item(List<T> property, int index, T value) => property[index] = value;

            protected override float DrawItem(Rect position, ITypedDrawer<T> drawer, ref T item, int index)
            {
                if (item == null) return EditorGUIUtility.singleLineHeight;
                var itemLabel = drawer.PagerItemName(item, index);
                var itemHeight = drawer.GetPropertyHeight(item, new GUIContent(itemLabel));
                TypedGUI.TypedField(position.SliceV(0, itemHeight), ref item, itemLabel);
                return itemHeight;
            }

            protected override bool FixedPropertyHeight(ITypedDrawer<T> drawer) => drawer.FixedPropertyHeight;
            protected override float GetPropertyHeight(ITypedDrawer<T> drawer, T item)
            {
                if (item == null) return EditorGUIUtility.singleLineHeight;
                return drawer.GetPropertyHeight(item, GUIContent.none);
            }
        }
    }
}