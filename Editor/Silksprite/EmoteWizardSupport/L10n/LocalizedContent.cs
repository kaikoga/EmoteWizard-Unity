using System;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizardSupport.L10n
{
    public readonly struct LocalizedContent : IEquatable<LocalizedContent>
    {
        readonly string _key;
        public LocalizedContent(string key) => _key = key;

        public bool IsNullOrEmpty() => string.IsNullOrEmpty(_key);

        public string Tr => Tr(_key);
        public string LongTr => LocalizationSetting.Nowrap ? Tr(_key).Nowrap() : Tr(_key);
        public GUIContent GUIContent => GUIContent(_key);

        public string TrFormat(Substitution substitution) => LocalizationTool.TrFormat(_key, substitution);
        public string LongTrFormat(Substitution substitution) => LocalizationSetting.Nowrap ? TrFormat(substitution).Nowrap() : TrFormat(substitution);

        public static bool operator ==(LocalizedContent left, LocalizedContent right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LocalizedContent left, LocalizedContent right)
        {
            return !left.Equals(right);
        }

        public bool Equals(LocalizedContent other)
        {
            return _key == other._key;
        }

        public override bool Equals(object obj)
        {
            return obj is LocalizedContent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (_key != null ? _key.GetHashCode() : 0);
        }
    }
}