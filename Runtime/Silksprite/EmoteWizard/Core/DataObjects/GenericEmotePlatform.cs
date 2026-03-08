using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    // FIXME: this should be type related to GenericEmoteTrigger
    public enum GenericEmotePlatform
    {
        [InspectorName("VRChat")] VRChat = 0,
        [InspectorName("ChilloutVR")] ChilloutVR = 1,
        [InspectorName("VRM0.x")] VRM0 = 0x100,
        [InspectorName("VRM1.0")] VRM1 = 0x101
    }

}