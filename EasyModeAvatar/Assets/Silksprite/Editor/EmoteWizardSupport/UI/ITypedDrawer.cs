using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public interface ITypedDrawer
    {
    }

    public interface ITypedDrawer<T> : ITypedDrawer
    {
        bool FixedPropertyHeight { get; }
        string PagerItemName(T property, int index);
        void OnGUI(Rect position, ref T property, GUIContent label);
        float GetPropertyHeight(T property, GUIContent label);
    }
}