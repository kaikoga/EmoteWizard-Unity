using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    public enum GenericEmotePlatform
    {
        [InspectorName("HandSigns")] HandSigns = 0,
        [InspectorName("VRM0.x")] VRM0 = 0x100,
        [InspectorName("VRM1.0")] VRM1 = 0x101
    }
}