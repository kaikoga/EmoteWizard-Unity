using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.References;

namespace Silksprite.EmoteWizard.Platforms
{
    public static partial class PlatformFeatures
    {
        static readonly IPlatformFeatures VRChatInstance = new VRChatFeatures(PlatformReferences.VRChatInstance);

        static readonly IPlatformFeatures ChilloutVRInstance = new ChilloutVRFeatures(PlatformReferences.ChilloutVRInstance);

        static readonly IPlatformFeatures MaybeVRChatInstance = new MaybeVRChatFeatures(PlatformReferences.VRChatInstance);

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
