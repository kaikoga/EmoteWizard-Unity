using System;

namespace Silksprite.EmoteWizard.DataObjects.Platforms
{
    public static partial class PlatformFeatures
    {
        [Obsolete("PlatformFeature.VRChat should be handled in a better way.")]
        public static readonly IPlatformFeatures VRChat = new VRChatFeatures();

        [Obsolete("PlatformFeature.Current should be handled in a better way.")]
        public static readonly IPlatformFeatures Current = VRChat;
    }

}
