using static Silksprite.EmoteWizard.PlatformConstants;

namespace Silksprite.EmoteWizard.Platforms.References
{
    static partial class PlatformReferences
    {
        class VRChatReferences : IPlatformReferences
        {
            string IPlatformReferences.VisemeReference => VRChat.Params.Viseme;
            string IPlatformReferences.ActionSelectReference => VRChat.Params.VRCEmote;
            string IPlatformReferences.GestureLeftReference => VRChat.Params.GestureLeft;
            string IPlatformReferences.GestureLeftWeightReference => VRChat.Params.GestureLeftWeight;
            string IPlatformReferences.GestureRightReference => VRChat.Params.GestureRight;
            string IPlatformReferences.GestureRightWeightReference => VRChat.Params.GestureRightWeight;
        }
    }
}
