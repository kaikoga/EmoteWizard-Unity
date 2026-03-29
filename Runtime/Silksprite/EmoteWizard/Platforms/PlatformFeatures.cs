using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.References;

namespace Silksprite.EmoteWizard.Platforms
{
    public static partial class PlatformFeatures
    {
        static readonly ChilloutVRFeatures ChilloutVRFromVRChat = new ChilloutVRFeatures(PlatformReferences.VRChatInstance);
        static readonly ChilloutVRFeatures ChilloutVRInstance = new ChilloutVRFeatures(PlatformReferences.ChilloutVRInstance);
        static readonly VRChatFeatures VRChatInstance = new VRChatFeatures(PlatformReferences.VRChatInstance);
        static readonly VRChatFeatures VRChatFromChilloutVR = new VRChatFeatures(PlatformReferences.ChilloutVRInstance);
        static readonly MaybeVRChatFeatures MixedInstance = new MaybeVRChatFeatures(PlatformReferences.VRChatInstance);
        static readonly MaybeVRChatFeatures MixedFromChilloutVR = new MaybeVRChatFeatures(PlatformReferences.ChilloutVRInstance);

        static readonly Dictionary<(DetectedPlatform, ParameterScheme), IPlatformFeatures> Instances = new Dictionary<(DetectedPlatform, ParameterScheme), IPlatformFeatures>
        {
            [(DetectedPlatform.ChilloutVR, ParameterScheme.VRChat)] = ChilloutVRFromVRChat,
            [(DetectedPlatform.ChilloutVR, ParameterScheme.ChilloutVR)] = ChilloutVRInstance,
            [(DetectedPlatform.ChilloutVR, ParameterScheme.Detected)] = ChilloutVRInstance,
            [(DetectedPlatform.VRChat, ParameterScheme.VRChat)] = VRChatInstance,
            [(DetectedPlatform.VRChat, ParameterScheme.ChilloutVR)] = VRChatFromChilloutVR,
            [(DetectedPlatform.VRChat, ParameterScheme.Detected)] = VRChatInstance,
            [(DetectedPlatform.Mixed, ParameterScheme.VRChat)] = MixedInstance,
            [(DetectedPlatform.Mixed, ParameterScheme.ChilloutVR)] = MixedFromChilloutVR,
            [(DetectedPlatform.Mixed, ParameterScheme.Detected)] = MixedInstance
        };

        public static IPlatformFeatures GetPlatformFeatures(EmoteWizardEnvironment env)
        {
            var detectedPlatform = (env.Platform.HasFlag(DetectedPlatform.VRChat), env.Platform.HasFlag(DetectedPlatform.ChilloutVR)) switch
            {
                (false, true) => DetectedPlatform.ChilloutVR,
                (true, false) => DetectedPlatform.VRChat,
                (_, _) => DetectedPlatform.Mixed
            };
            return Instances[(detectedPlatform, env.ParameterScheme)];
        }
    }
}
