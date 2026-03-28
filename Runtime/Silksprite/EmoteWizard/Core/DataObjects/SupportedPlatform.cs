using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Silksprite.EmoteWizard.DataObjects
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class SupportedPlatform
    {
#if EW_VRCSDK3_AVATARS
        public const bool VRCSDK3_AVATARS = true;
#else
        public const bool VRCSDK3_AVATARS = false;
#endif
            
#if CVR_CCK_EXISTS || ADLIB_CVR_CCK_STUBBED
        public const bool CHILLOUTVR_AVATARS = true;
#else
        public const bool CHILLOUTVR_AVATARS = false;
#endif
            
#if ATIV_DETECTED_VRM0
        public const bool VRM0 = true;
#else
        public const bool VRM0 = false;
#endif
            
#if ATIV_DETECTED_VRM1
        public const bool VRM1 = true;
#else
        public const bool VRM1 = false;
#endif

        public const bool VRM = VRM0 || VRM1;

        public static readonly bool IsMultiple = new [] {VRCSDK3_AVATARS, CHILLOUTVR_AVATARS, VRM0, VRM1}.Count(b => b) > 1;
    }
}
