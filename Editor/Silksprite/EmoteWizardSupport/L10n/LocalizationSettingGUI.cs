using System;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationSetting;

namespace Silksprite.EmoteWizardSupport.L10n
{
    public static class LocalizationSettingGUI
    {
        public static void LocalizationSelector()
        {
            using (var changed = new EditorGUI.ChangeCheckScope())
            {
                var lang = EditorGUILayout.Popup(Loc("EWS::Language").Tr, Array.IndexOf(AllLangs, Lang), DisplayAllLangs);
                if (changed.changed && lang >= 0) Lang = AllLangs[lang];
            }
        }
    }
}