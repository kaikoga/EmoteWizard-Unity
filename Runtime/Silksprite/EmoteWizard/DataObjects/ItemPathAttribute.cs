using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    public class ItemPathAttribute : PropertyAttribute
    {
        public static bool IsInvalidPathInput(string value) => string.IsNullOrWhiteSpace(value) || value.StartsWith("/") || value.EndsWith("/");
    }
}