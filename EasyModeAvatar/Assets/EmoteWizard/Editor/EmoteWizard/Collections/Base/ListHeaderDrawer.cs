using EmoteWizard.Extensions;
using EmoteWizard.UI;
using UnityEngine;
using static EmoteWizard.Tools.PropertyDrawerUITools;

namespace EmoteWizard.Collections.Base
{
    public abstract class ListHeaderDrawer
    {
        public void OnGUI(bool useReorderUI)
        {
            var position = GUILayoutUtility.GetRect(0, GetHeaderHeight());
            OnGUI(position, useReorderUI);
        }

        private void OnGUI(Rect position, bool useReorderUI)
        {
            EmoteWizardGUI.ColoredBox(position, Color.yellow);
            position = position.InsideBox();
            position.xMin += useReorderUI ? 20f : 6f;
            position.xMax -= 6f;
            
            DrawHeaderContent(position);
        }

        protected virtual void DrawHeaderContent(Rect position)
        {
            
        }

        public virtual float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(1f));
        }
    }
}