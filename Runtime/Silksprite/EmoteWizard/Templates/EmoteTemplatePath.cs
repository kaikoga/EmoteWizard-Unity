namespace Silksprite.EmoteWizard.Templates
{
    public readonly struct EmoteTemplatePath
    {
        public readonly string RelativePath;

        EmoteTemplatePath(string relativePath)
        {
            RelativePath = relativePath;
        }

        public static EmoteTemplatePath Relative(string relativePath)
        {
            return new EmoteTemplatePath(relativePath);
        }
    }
}