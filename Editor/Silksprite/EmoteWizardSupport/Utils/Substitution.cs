using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Silksprite.EmoteWizardSupport.Utils
{
    public class Substitution
    {
        static readonly Regex Pattern = new Regex("{([^}]*)}");
        
        public static readonly Substitution Empty = new Substitution(s => s);

        readonly Func<string, string> _mapping;
        Substitution(Func<string, string> mapping) => _mapping = mapping;

        public string Format(string value)
        {
            var mapping = _mapping;
            return Pattern.Replace(value, match => mapping(match.Groups[1].Value));
        }

        public static implicit operator Substitution(Func<string, string> mapping) => new Substitution(mapping);
        public static implicit operator Substitution(Dictionary<string, string> dict)
        {
            return new Substitution(key => dict.TryGetValue(key, out var value) ? value : $"{{{key}}}");
        }
    }
}