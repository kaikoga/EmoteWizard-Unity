using Silksprite.Loch.Attributes;

namespace Silksprite.EmoteWizard.Contexts
{
    [LEnum]
    public enum OverrideGeneratedControllerType1
    {
        Generate = 0x10,
        Override = 0x11,
        Default = 0x00,
        Inherit = 0x7f
    }

    [LEnum]
    public enum OverrideGeneratedControllerType2
    {
        Generate = 0x10,
        Override = 0x11,
        Default1 = 0x00,
        Default2 = 0x01,
        Inherit = 0x7f
    }

    [LEnum]
    public enum OverrideControllerType2
    {
        Override = 0x11,
        Default1 = 0x00,
        Default2 = 0x01,
        Inherit = 0x7f
    }
}