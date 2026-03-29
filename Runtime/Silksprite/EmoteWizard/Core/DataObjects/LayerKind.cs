using Silksprite.Loch.Attributes;

namespace Silksprite.EmoteWizard.DataObjects
{
    [LEnum]
    public enum LayerKind
    {
        None,
        FX, // this is strictly "FX" to match VRC Avatar SDK Editor UI
        Gesture,
        Action
    }
}