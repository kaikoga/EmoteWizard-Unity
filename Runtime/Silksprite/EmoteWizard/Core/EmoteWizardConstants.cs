using System.Linq;

namespace Silksprite.EmoteWizard
{
    public static class EmoteWizardConstants
    {
        public static class Platforms
        {
#if EW_VRCSDK3_AVATARS
            public const bool VRCSDK3_AVATARS = true;
#else
            public const bool VRCSDK3_AVATARS = false;
#endif
            
#if EW_VRM0
            public const bool VRM0 = true;
#else
            public const bool VRM0 = false;
#endif
            
#if EW_VRM1
            public const bool VRM1 = true;
#else
            public const bool VRM1 = false;
#endif

            public const bool VRM = VRM0 || VRM1;

            public static readonly bool IsMultiple = new [] {VRCSDK3_AVATARS, VRM0, VRM1}.Count(b => b) > 1;
        }

        public static class LayerNames
        {
            public const string Gesture = "Gesture";
            public const string Fx = "FX";
            public const string Action = "Action";
        }

        public static class Params
        {
            public const string Viseme = "Viseme";
            public const string AFK = "AFK";
            public const string Gesture = "Gesture";
            public const string GestureOther = "GestureOther";
            public const string GestureWeight = "GestureWeight";
            public const string GestureOtherWeight = "GestureOtherWeight";

            public static bool IsMirrorParameter(string parameter)
            {
                switch (parameter)
                {
                    case Gesture:
                    case GestureOther:
                    case GestureWeight:
                    case GestureOtherWeight:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public static class Defaults
        {
            public static class Groups
            {
                public const string HandSign = "HandSign";
                public const string Action = "Action";
            }

            public static class Params
            {
                public const string GestureHandSignOverride = "EmoteWizardGesture";
                public const string FxHandSignOverride = "EmoteWizardFx";
                public const string ActionSelect = "VRCEmote";
                public const string AfkSelect = "EmoteWizardAFK";
            }
        }
    }
}