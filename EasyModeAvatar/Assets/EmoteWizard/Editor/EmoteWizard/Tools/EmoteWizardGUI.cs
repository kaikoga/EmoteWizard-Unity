using System;
using EmoteWizard.Base;
using EmoteWizard.Scopes;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class EmoteWizardGUI
    {
        public static void ColoredBox(Rect position, Color color)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(position, GUIContent.none);
            GUI.backgroundColor = backgroundColor;
        }
    }
}