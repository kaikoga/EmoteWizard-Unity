namespace Silksprite.EmoteWizard.Utils
{
    public static class ItemPathUtil
    {
        public static bool IsInvalidPathFormat(string value) => string.IsNullOrWhiteSpace(value) || value.StartsWith("/") || value.EndsWith("/");
    }

}
