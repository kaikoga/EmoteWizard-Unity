using Silksprite.EmoteWizard.Contexts;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    public static partial class PlatformFeatures
    {
        static readonly IPlatformFeatures VRChat = new VRChatFeatures();

        static readonly IPlatformFeatures ChilloutVR = new ChilloutVRFeatures();

        public static IPlatformFeatures Of(EmoteWizardEnvironment env)
        {
            return (env.Platform.HasFlag(DetectedPlatform.VRChat), env.Platform.HasFlag(DetectedPlatform.ChilloutVR)) switch
            {
                (false, true) => ChilloutVR,
                (_, _) => VRChat
            };
        }
    }
}
