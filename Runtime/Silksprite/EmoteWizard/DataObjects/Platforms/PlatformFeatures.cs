using System;
using Silksprite.EmoteWizard.Contexts;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    public static partial class PlatformFeatures
    {
        [Obsolete("PlatformFeature.VRChat should be handled in a better way.")]
        public static readonly IPlatformFeatures VRChat = new VRChatFeatures();

        static readonly IPlatformFeatures ChilloutVR = new ChilloutVRFeatures();

        [Obsolete("PlatformFeature.Current should be handled in a better way.")]
        public static readonly IPlatformFeatures Current = VRChat;

        public static IPlatformFeatures Of(EmoteWizardEnvironment env)
        {
 #pragma warning disable CS0618 // Type or member is obsolete
            return !env.Platform.HasFlag(DetectedPlatform.VRChat) && env.Platform.HasFlag(DetectedPlatform.ChilloutVR) ? ChilloutVR : VRChat;
 #pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
