using static Silksprite.EmoteWizard.EmoteWizardConstants;
using static Silksprite.EmoteWizard.PlatformConstants;

namespace Silksprite.EmoteWizard.Platforms.References
{
    static partial class PlatformReferences
    {
        class ChilloutVRReferences : IPlatformReferences
        {
            string IPlatformReferences.VisemeReference => ChilloutVR.Params.VisemeIdx;
            string IPlatformReferences.ActionSelectReference => Params.ActionSelect;
            string IPlatformReferences.GestureLeftReference => ChilloutVR.Params.GestureLeftIdx;
            string IPlatformReferences.GestureLeftWeightReference => ChilloutVR.Params.GestureLeft;
            string IPlatformReferences.GestureRightReference => ChilloutVR.Params.GestureRightIdx;
            string IPlatformReferences.GestureRightWeightReference => ChilloutVR.Params.GestureRight;
        }
    }
}
