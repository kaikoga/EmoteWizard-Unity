using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public static class EmoteWizardEnvironmentExtension
    {
        public static bool MaybeVRChat(this EmoteWizardEnvironment env) => env.Platform.HasFlag(DetectedPlatform.VRChat);
        public static bool MaybeChilloutVR(this EmoteWizardEnvironment env) => env.Platform.HasFlag(DetectedPlatform.ChilloutVR);
        public static bool MaybeUnityPlatforms(this EmoteWizardEnvironment env) => env.MaybeVRChat() || env.MaybeChilloutVR();
        public static bool MaybeVRM0(this EmoteWizardEnvironment env) => env.Platform.HasFlag(DetectedPlatform.VRM0);
        public static bool MaybeVRM1(this EmoteWizardEnvironment env) => env.Platform.HasFlag(DetectedPlatform.VRM1);
        public static bool MaybeVRM(this EmoteWizardEnvironment env) => env.MaybeVRM0() || env.MaybeVRM1();
        
        public static bool IsVRChatAvatar(this EmoteWizardEnvironment env) => env.MaybeVRChat() && env.AvatarRoot;
        public static bool IsChilloutVRAvatar(this EmoteWizardEnvironment env) => env.MaybeChilloutVR() && env.AvatarRoot;
        public static bool IsVRM0Avatar(this EmoteWizardEnvironment env) => env.MaybeVRM0() && env.AvatarRoot;
        public static bool IsVRM1Avatar(this EmoteWizardEnvironment env) => env.MaybeVRM1() && env.AvatarRoot;
        public static bool IsVRMAvatar(this EmoteWizardEnvironment env) => env.MaybeVRM() && env.AvatarRoot;

    }
}