using System;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    public class PlatformFeatures
    {
        [Obsolete("PlatformFeature.Current should be handled in a better way.")]
        public static readonly PlatformFeatures Current = new PlatformFeatures();
    }
}
