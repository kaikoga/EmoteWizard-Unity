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

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedDrawerRegistry
    {
        class InvalidDrawer : TypedDrawerBase<object>
        {
            public override void OnGUI(Rect position, ref object property, GUIContent label)
            {
                EditorGUI.LabelField(position, label, new GUIContent($"{property?.GetType().Name ?? "Null"} Drawer"));
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

        abstract class CollectionDrawerBase<TList, TDrawer> : TypedDrawerBase<TList>
            where TList : IList
        {
            protected abstract void ResizeAndPopulate(ref TList property, int arraySize);
            protected abstract TDrawer SharedDrawer(TList property);
            protected abstract void ItemDrawer(ref TDrawer drawer, TList property, int index);
            protected abstract float DrawItem(Rect position, TDrawer drawer, ref TList property, int index);
            protected abstract bool FixedPropertyHeight(TDrawer drawer);
            protected abstract float GetPropertyHeight(TDrawer drawer, TList property, int index);

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
                    ItemDrawer(ref drawer, property, i);
                    var height = DrawItem(position, drawer, ref property, i) + EditorGUIUtility.standardVerticalSpacing;
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
                    height += PropertyDrawerUITools.LineTop(property.Count * GetPropertyHeight(drawer, property, 0));
                }
                else
                {
                    for (var i = 0; i < property.Count; i++)
                    {
                        height += GetPropertyHeight(drawer, property, i) + EditorGUIUtility.standardVerticalSpacing;
                    }
                } 

                return height;
            }
        }

        [UsedImplicitly]
        class ListDrawer : CollectionDrawerBase<IList, UntypedDrawer>
        {
            protected override void ResizeAndPopulate(ref IList property, int arraySize) => ListUtils.ResizeAndPopulate(ref property, arraySize);
            protected override UntypedDrawer SharedDrawer(IList property) => null;
            protected override void ItemDrawer(ref UntypedDrawer drawer, IList property, int index) => drawer = Drawer(property[index]?.GetType());

            protected override float DrawItem(Rect position, UntypedDrawer drawer, ref IList property, int index)
            {
                var item = property[index];
                var itemLabel = drawer.UntypedPagerItemName(item, index);
                var itemHeight = drawer.UntypedGetPropertyHeight(item, new GUIContent(itemLabel));
                TypedGUI.UntypedField(position.SliceV(0, itemHeight), ref item, itemLabel);
                property[index] = item;
                return itemHeight;
            }

            protected override bool FixedPropertyHeight(UntypedDrawer drawer) => false;
            protected override float GetPropertyHeight(UntypedDrawer drawer, IList property, int index)
            {
                return drawer.UntypedGetPropertyHeight(property.Count > 0 ? property[index] : default, GUIContent.none);
            }
        }

        [UsedImplicitly]
        class ArrayDrawer<T> : CollectionDrawerBase<T[], ITypedDrawer<T>>
        {
            protected override void ResizeAndPopulate(ref T[] property, int arraySize) => ArrayUtils.ResizeAndPopulate(ref property, arraySize);
            protected override ITypedDrawer<T> SharedDrawer(T[] property) => TypedDrawerRegistry<T>.Drawer;
            protected override void ItemDrawer(ref ITypedDrawer<T> drawer, T[] property, int index) { }

            protected override float DrawItem(Rect position, ITypedDrawer<T> drawer, ref T[] property, int index)
            {
                var item = property[index];
                var itemLabel = drawer.PagerItemName(item, index);
                var itemHeight = drawer.GetPropertyHeight(item, new GUIContent(itemLabel));
                TypedGUI.TypedField(position.SliceV(0, itemHeight), ref item, itemLabel);
                property[index] = item;
                return itemHeight;
            }

            protected override bool FixedPropertyHeight(ITypedDrawer<T> drawer) => drawer.FixedPropertyHeight;
            protected override float GetPropertyHeight(ITypedDrawer<T> drawer, T[] property, int index)
            {
                return drawer.GetPropertyHeight(property.Length > 0 ? property[index] : default, GUIContent.none);
            }
        }
        
        [UsedImplicitly]
        class ListDrawer<T> : CollectionDrawerBase<List<T>, ITypedDrawer<T>>
        {
            protected override void ResizeAndPopulate(ref List<T> property, int arraySize) => ListUtils.ResizeAndPopulate(ref property, arraySize);
            protected override ITypedDrawer<T> SharedDrawer(List<T> property) => TypedDrawerRegistry<T>.Drawer;
            protected override void ItemDrawer(ref ITypedDrawer<T> drawer, List<T> property, int index) { }

            protected override float DrawItem(Rect position, ITypedDrawer<T> drawer, ref List<T> property, int index)
            {
                var item = property[index];
                var itemLabel = drawer.PagerItemName(item, index);
                var itemHeight = drawer.GetPropertyHeight(item, new GUIContent(itemLabel));
                TypedGUI.TypedField(position.SliceV(0, itemHeight), ref item, itemLabel);
                property[index] = item;
                return itemHeight;
            }

            protected override bool FixedPropertyHeight(ITypedDrawer<T> drawer) => drawer.FixedPropertyHeight;
            protected override float GetPropertyHeight(ITypedDrawer<T> drawer, List<T> property, int index)
            {
                return drawer.GetPropertyHeight(property.Count > 0 ? property[index] : default, GUIContent.none);
            }
        }
    }
}