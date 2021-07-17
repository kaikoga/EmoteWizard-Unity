using System;
using System.Collections;
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
                EditorGUI.LabelField(position, label, new GUIContent($"{property?.GetType().Name} Drawer"));
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
        class ListDrawer : TypedDrawerBase<IList>
        {
            public override void OnGUI(Rect position, ref IList property, GUIContent label)
            {
                if (!TypedGUI.Foldout(position.UISliceV(0), property, label)) return;

                const int arraySizeMax = 100;
                var arraySize = property.Count;
                TypedGUI.DelayedIntField(position.UISliceV(1), "Size", ref arraySize);
                if (arraySize > arraySizeMax) arraySize = arraySizeMax;
                ListUtils.ResizeAndPopulate(ref property, arraySize);
                position = position.UISliceV(2, -2);
                for (var i = 0; i < property.Count; i++)
                {
                    var item = property[i];
                    var drawer = Drawer(item?.GetType());
                    var itemLabel = drawer.UntypedPagerItemName(item, i);
                    var itemHeight = drawer.UntypedGetPropertyHeight(item, new GUIContent(itemLabel));
                    TypedGUI.UntypedField(position.SliceV(0, itemHeight), ref item, itemLabel);
                    property[i] = item;
                    itemHeight += EditorGUIUtility.standardVerticalSpacing;
                    position = position.SliceV(itemHeight, -itemHeight);
                }
            }

            public override float GetPropertyHeight(IList property, GUIContent label)
            {
                if (!IsExpandedTracker.GetIsExpanded(property))
                {
                    return EditorGUIUtility.singleLineHeight;                    
                }

                return PropertyDrawerUITools.LineHeight(2f + property.Count);
            }
        }
    }
}