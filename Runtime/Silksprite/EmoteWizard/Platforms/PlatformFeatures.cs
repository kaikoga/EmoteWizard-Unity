using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Platforms
{
    public static partial class PlatformFeatures
    {
        static readonly IPlatformFeatures VRChat = new VRChatFeatures();

        static readonly IPlatformFeatures ChilloutVR = new ChilloutVRFeatures();

        static readonly IPlatformFeatures MaybeVRChat = new MaybeVRChatFeatures();

        public static IPlatformFeatures GetPlatformFeatures(EmoteWizardEnvironment env)
        {
            return (env.Platform.HasFlag(DetectedPlatform.VRChat), env.Platform.HasFlag(DetectedPlatform.ChilloutVR)) switch
            {
                (false, true) => ChilloutVR,
                (true, false) => VRChat,
                _ => MaybeVRChat
            };
        }
    }
}
