using System;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedDrawerRegistry
    {
        public abstract class UntypedDrawer
        {
            public abstract UntypedDrawer Subtype(Type type);  
            public abstract ITypedDrawer Typed { get; }
            public virtual bool UntypedFixedPropertyHeight => true;

            public virtual string UntypedPagerItemName(object property, int index) => $"Item {index + 1}";
            public abstract void UntypedOnGUI(Rect position, ref object property, GUIContent label);
            public virtual float UntypedGetPropertyHeight(object property, GUIContent label) => EditorGUIUtility.singleLineHeight;
        }

        class UntypedDrawer<T> : UntypedDrawer
        {
            readonly ITypedDrawer<T> _drawer;

            public override UntypedDrawer Subtype(Type type)
            {
                object drawer = _drawer;
                if (!drawer.GetType().IsGenericType) return this;
                    
                var drawerType = drawer.GetType().GetGenericTypeDefinition().MakeGenericType(type);
                drawer = Activator.CreateInstance(drawerType);
                var untypedDrawerType = typeof(UntypedDrawer<>).MakeGenericType(type);
                return (UntypedDrawer) untypedDrawerType.GetConstructor(new []{typeof(ITypedDrawer<>).MakeGenericType(type)})
                    .Invoke(new []{drawer});
            }

            public override ITypedDrawer Typed => _drawer;
            public override bool UntypedFixedPropertyHeight => _drawer.FixedPropertyHeight;

            public UntypedDrawer(ITypedDrawer<T> drawer) => _drawer = drawer;

            public override string UntypedPagerItemName(object property, int index) => _drawer.PagerItemName((T) property, index);

            public override void UntypedOnGUI(Rect position, ref object property, GUIContent label)
            {
                var value = (T) property;
                _drawer.OnGUI(position, ref value, label);
                property = value;
            }

            public override float UntypedGetPropertyHeight(object property, GUIContent label) => _drawer.GetPropertyHeight((T) property, label);
        }
    }
}