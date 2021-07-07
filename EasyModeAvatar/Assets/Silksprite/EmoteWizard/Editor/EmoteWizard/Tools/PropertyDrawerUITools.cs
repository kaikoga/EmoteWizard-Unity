using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Tools
{
    public static class PropertyDrawerUITools
    {
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
    }
}