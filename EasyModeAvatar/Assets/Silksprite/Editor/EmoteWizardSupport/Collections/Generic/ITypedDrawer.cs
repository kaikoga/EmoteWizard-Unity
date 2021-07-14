using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Collections.Generic
{
    public interface ITypedDrawer<in T>
    {
        string PagerItemName(T property, int index);
        void OnGUI(Rect position, T property, GUIContent label);
        float GetPropertyHeight(T property, GUIContent label);
    }
}