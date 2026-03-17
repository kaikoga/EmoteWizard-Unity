using Silksprite.EmoteWizard.Contexts;

namespace Silksprite.EmoteWizard.Platforms.Extensions
{
    public static class PlatformFeaturesExtension
    {
        public static IPlatformFeatures GetPlatformFeatures(this EmoteWizardEnvironment env) => PlatformFeatures.GetPlatformFeatures(env);
    }
}
