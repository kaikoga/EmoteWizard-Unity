using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Platforms
{
    public static partial class PlatformFeatures
    {
        static readonly IPlatformFeatures VRChatInstance = new VRChatFeatures();

        static readonly IPlatformFeatures ChilloutVRInstance = new ChilloutVRFeatures();

        static readonly IPlatformFeatures MaybeVRChatInstance = new MaybeVRChatFeatures();

        public static IPlatformFeatures GetPlatformFeatures(EmoteWizardEnvironment env)
        {
            return (env.Platform.HasFlag(DetectedPlatform.VRChat), env.Platform.HasFlag(DetectedPlatform.ChilloutVR)) switch
            {
                (false, true) => ChilloutVRInstance,
                (true, false) => VRChatInstance,
                _ => MaybeVRChatInstance
            };
        }
    }
}
