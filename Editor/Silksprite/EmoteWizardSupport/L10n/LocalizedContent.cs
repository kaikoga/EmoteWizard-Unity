using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizardSupport.L10n
{
    public readonly struct LocalizedContent
    {
        readonly string _key;
        public LocalizedContent(string key) => _key = key;

        public bool IsNullOrEmpty() => string.IsNullOrEmpty(_key);

        public string Tr => Tr(_key);
        public string LongTr => LocalizationSetting.Nowrap ? Tr(_key).Nowrap() : Tr(_key);
        public GUIContent GUIContent => GUIContent(_key);

        public string TrFormat(Substitution substitution) => LocalizationTool.TrFormat(_key, substitution);
    }
}