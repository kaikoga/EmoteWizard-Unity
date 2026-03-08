using System;

namespace Silksprite.EmoteWizard.DataObjects
{
    // FIXME: maybe move to PlatformFeature
    [Flags]
    public enum DetectedPlatform
    {
        None = 0,
        VRChat = 0x1,
        ChilloutVR = 0x2,
        UnityPlatforms = 0x3,
        VRM0 = 0x100,
        VRM1 = 0x200,
        VRMPlatforms = 0x300,
        Mixed = -1
    }

    public static class DetectedPlatformExtension
    {
        public static string ToSolePlatformString(this DetectedPlatform platform)
        {
            switch (platform)
            {
                case DetectedPlatform.VRChat:
                    return "VRChat";
                case DetectedPlatform.ChilloutVR:
                    return "ChilloutVR";
                case DetectedPlatform.VRM0:
                    return "VRM0.x";
                case DetectedPlatform.VRM1:
                    return "VRM1.0";
                default:
                    return "Unknown";
            }
        }
    }
}
