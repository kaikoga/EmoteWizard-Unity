using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI.Base
{
    public abstract class TypedDrawerBase<T> : ITypedDrawer<T>
    {
        public virtual bool FixedPropertyHeight => true;
        public virtual bool DrawNullCollectionItem => false;

        public virtual string PagerItemName(T property, int index) => $"Item {index + 1}";

        public abstract void OnGUI(Rect position, ref T property, GUIContent label);

        public virtual float GetPropertyHeight(T property, GUIContent label) => EditorGUIUtility.singleLineHeight;
    }
}