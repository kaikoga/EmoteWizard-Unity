using EmoteWizard.Extensions;
using EmoteWizard.UI;
using UnityEditor;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.Collections.Base
{
    public abstract class ListDrawerBase
    {
        public abstract string HeaderName { get; }
        public abstract string PagerItemName(SerializedProperty property, int index);

        public virtual float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(1f));
        }

        protected abstract void DrawHeaderContent(Rect position);

        public void OnGUI(bool useReorderUI)
        {
            var position = GUILayoutUtility.GetRect(0, GetHeaderHeight());
            OnGUI(position, useReorderUI);
        }

        public void OnGUI(Rect position, bool useReorderUI)
        {
            EmoteWizardGUI.ColoredBox(position, Color.yellow);
            position = position.InsideBox();
            position.xMin += useReorderUI ? 20f : 6f;
            position.xMax -= 6f;
            
            DrawHeaderContent(position);
        }
    }
}