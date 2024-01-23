using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    public class ParameterNameAttribute : PropertyAttribute
    {
        public static bool IsInvalidParameterInput(string value, bool allowEmpty) => (!allowEmpty && string.IsNullOrWhiteSpace(value)) || value.Contains("/");


        public readonly bool AllowEmpty;
        public readonly bool AllowNew;

        public ParameterNameAttribute(bool allowEmpty, bool allowNew)
        {
            AllowEmpty = allowEmpty;
            AllowNew = allowNew;
        }
    }
}