using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.L10n
{
    public static class LocalizationTool
    {
        static LocalizationAsset Po => PoCache.FindOrCreate("po", (lang, key) => AssetDatabase.LoadAssetAtPath<LocalizationAsset>(LocalizationSetting.PoPath(lang)));
        static readonly LocalizationCache<LocalizationAsset> PoCache = new LocalizationCache<LocalizationAsset>();

        public static string Tr(string key) => TrCache.FindOrCreate(key, GetTr);
        static string GetTr(string lang, string key)
        {
            if (Po == null) return key.SplitCompat("::").LastOrDefault();
            var str = Po.GetLocalizedString(key);
            return key == str ? $"<{str}>" : str;
        }

        public static string TrFormat(string key, Substitution sub) => sub.Format(Tr(key));

        public static GUIContent GUIContent(string key) => GUIContentCache.FindOrCreate(key, GetGUIContent);
        static GUIContent GetGUIContent(string lang, string key)
        {
            var tr = Tr(key);
            return new GUIContent(tr, null, tr);
        }

        static readonly LocalizationCache<string> TrCache = new LocalizationCache<string>();
        static readonly LocalizationCache<GUIContent> GUIContentCache = new LocalizationCache<GUIContent>();
        public static LocalizedContent Loc(string key) => new LocalizedContent(key);
        public static LocalizedContent _Loc(string key) => new LocalizedContent(key);

        class LocalizationCache<T>
        {
            string _lang;
            Dictionary<string, T> _cache = new Dictionary<string, T>();

            public delegate T Generator(string lang, string key);

            public void Clear()
            {
                _lang = null;
                _cache.Clear();
            }

            public T FindOrCreate(string key, Generator generator)
            {
                if (_lang != LocalizationSetting.Lang)
                {
                    _lang = LocalizationSetting.Lang;
                    _cache = new Dictionary<string, T>();
                }
                if (_cache.TryGetValue(key, out var value)) return value;
                value = generator(_lang, key);
                _cache.Add(key, value);
                return value;
            }
        }

        class LocalizationPostProcessor : AssetPostprocessor
        {
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
            {
                PoCache.Clear();
                TrCache.Clear();
                GUIContentCache.Clear();
            }
        }
    }
}