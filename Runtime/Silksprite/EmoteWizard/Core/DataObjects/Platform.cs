using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    public enum Platform
    {
        [InspectorName("VRChat")] VRChat = 0,
        [InspectorName("ChilloutVR")] ChilloutVR = 1,
        [InspectorName("VRM0.x")] VRM0 = 0x100,
        [InspectorName("VRM1.0")] VRM1 = 0x101
    }

    [Flags]
    public enum DetectedPlatform
    {
        None = 0,
        VRChat = 0x1,
        ChilloutVR = 0x2,
        VRM0 = 0x100,
        VRM1 = 0x200,
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