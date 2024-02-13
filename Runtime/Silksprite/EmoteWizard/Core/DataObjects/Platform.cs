using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    public enum Platform
    {
        [InspectorName("VRChat")] VRChat = 0,
        [InspectorName("VRM0.x")] VRM0 = 0x100,
        [InspectorName("VRM1.0")] VRM1 = 0x101
    }

    [Flags]
    public enum DetectedPlatform
    {
        None = 0,
        VRChat = 1,
        VRM0 = 0x100,
        VRM1 = 0x200,
        Mixed = -1
    }
    
    public static class DetectedPlatformExtension
    {
        public static bool IsVRChat(this DetectedPlatform platform) => platform.HasFlag(DetectedPlatform.VRChat);
        public static bool IsVRM0(this DetectedPlatform platform) => platform.HasFlag(DetectedPlatform.VRM0);
        public static bool IsVRM1(this DetectedPlatform platform) => platform.HasFlag(DetectedPlatform.VRM1);
        public static bool IsVRM(this DetectedPlatform platform) => platform.HasFlag(DetectedPlatform.VRM0) ||  platform.HasFlag(DetectedPlatform.VRM1);

        public static string ToSolePlatformString(this DetectedPlatform platform)
        {
            switch (platform)
            {
                case DetectedPlatform.VRChat:
                    return "VRChat";
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