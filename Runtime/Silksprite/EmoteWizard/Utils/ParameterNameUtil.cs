namespace Silksprite.EmoteWizard.Utils
{
    public static class ParameterNameUtil
    {
        public static bool IsInvalidParameterFormat(string value, bool allowEmpty) => (!allowEmpty && string.IsNullOrWhiteSpace(value)) || value.Contains("/");
    }
}
