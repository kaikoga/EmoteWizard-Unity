using System;
using EmoteWizard.Base;
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
    }
}