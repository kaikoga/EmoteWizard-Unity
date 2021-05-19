using System;
using EmoteWizard.Base;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class EditorUITools
    {
        public static void SetupOnlyUI(EmoteWizardBase emoteWizardBase, Action action)
        {
            if (!emoteWizardBase.IsSetupMode) return;
            
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                GUI.backgroundColor = Color.magenta;
                action();
            }
            GUI.backgroundColor = backgroundColor;
        }

        public static float LineHeight(float line, float dSpacing = -1f)
        {
            return EditorGUIUtility.singleLineHeight * line +
                   EditorGUIUtility.standardVerticalSpacing * (line + dSpacing);
        }
        
        public static float LineTop(float line, float dSpacing = 0f)
        {
            return EditorGUIUtility.singleLineHeight * line +
                   EditorGUIUtility.standardVerticalSpacing * (line + dSpacing);
        }

        public static float BoxHeight(float height)
        {
            return height + GUI.skin.box.padding.vertical * 2;
        }

        public static Rect InsideBox(Rect rect)
        {
            var border = GUI.skin.box.border;
            var padding = GUI.skin.box.padding;
            return new Rect(rect.x + border.left + padding.left,
                rect.y + border.top + padding.top,
                rect.width - border.horizontal - padding.horizontal,
                rect.height - border.vertical - padding.vertical);
        }
    }
}