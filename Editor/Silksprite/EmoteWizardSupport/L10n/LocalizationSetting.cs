using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;

#if EW_NDMF_SUPPORT
using nadena.dev.ndmf.localization;
#endif

namespace Silksprite.EmoteWizardSupport.L10n
{
    public static class LocalizationSetting
    {
        public static string[] AllLangs = { "en-US", "ja-JP", "CSharp" };

        static HashSet<string> NowrapLangs = new HashSet<string> { "ja-JP" };

        static string[] _displayAllLangs; 
        public static string[] DisplayAllLangs
        {
            get
            {
                return _displayAllLangs = _displayAllLangs ?? AllLangs
                    .Select(lang =>
                    {
                        try
                        {
                            return CultureInfo.GetCultureInfo(lang).NativeName;
                        }
                        catch (Exception)
                        {
                            return lang;
                        }
                    })
                    .ToArray();
            }
        }

#if EW_NDMF_SUPPORT
        public static string Lang
        {
            get => LanguagePrefs.Language;
            set => LanguagePrefs.Language = value;
        }
#else
        private const string EditorPrefKey = "net.kaikoga.emotewizard.lang";

        [InitializeOnLoadMethod]
        private static void Init()
        {
            _lang = EditorPrefs.GetString(EditorPrefKey, "en-US");
        }

        static string _lang = "en-US";
        public static string Lang
        {
            get => _lang;
            set
            {
                if (value == _lang) return;
                _lang = value;
                EditorPrefs.SetString(EditorPrefKey, value);
            }
        }
#endif

        public static bool Nowrap => NowrapLangs.Contains(Lang);

        internal static string PoPath(string lang)
        {
            return $"Packages/net.kaikoga.emotewizard/Locales/{lang}.po";
        }
    }
}