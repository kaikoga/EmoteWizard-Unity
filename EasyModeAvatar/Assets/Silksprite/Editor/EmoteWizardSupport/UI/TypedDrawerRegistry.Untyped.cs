using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedDrawerRegistry
    {
        public abstract class UntypedDrawer
        {
            public abstract ITypedDrawer typed { get; }
            public virtual bool UntypedFixedPropertyHeight => true;

            public virtual string UntypedPagerItemName(object property, int index) => $"Item {index + 1}";
            public abstract void UntypedOnGUI(Rect position, ref object property, GUIContent label);
            public virtual float UntypedGetPropertyHeight(object property, GUIContent label) => EditorGUIUtility.singleLineHeight;
        }

        class UntypedDrawer<T> : UntypedDrawer
        {
            readonly ITypedDrawer<T> _drawer;

            public override ITypedDrawer typed => _drawer;
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