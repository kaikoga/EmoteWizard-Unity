using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Collections.Generic.Base
{
    public abstract class TypedDrawerBase<T> : PropertyDrawer, ITypedDrawer<T>
    {
        public abstract string PagerItemName(T property, int index);
        public abstract void OnGUI(Rect position, T property, GUIContent label);
        public abstract float GetPropertyHeight(T property, GUIContent label);
    }
}