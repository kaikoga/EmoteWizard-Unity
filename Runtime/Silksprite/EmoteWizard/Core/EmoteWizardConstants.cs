namespace Silksprite.EmoteWizard
{
    public static class EmoteWizardConstants
    {

        public static class Params
        {
            public const string Viseme = "Viseme";
            public const string VisemeIdx = "VisemeIdx";
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
                public const string LipSync = "LipSync";
                public const string Blink = "Blink";
                public const string LookAt = "LookAt";
                public const string Emotion = "Emotion";
            }

            public static class Params
            {
                public const string ActionSelect = "VRCEmote";
            }
        }
    }
}