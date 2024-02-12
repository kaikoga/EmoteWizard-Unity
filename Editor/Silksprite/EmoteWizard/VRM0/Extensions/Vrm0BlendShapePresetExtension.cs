#if EW_VRM0

using Silksprite.EmoteWizard.DataObjects;
using VRM;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class Vrm0BlendShapePresetExtension
    {
        public static BlendShapePreset ToBlendShapePreset(this Vrm0BlendShapePreset vrm0BlendShape)
        {
            switch (vrm0BlendShape)
            {
                case Vrm0BlendShapePreset.Unknown:
                    return BlendShapePreset.Unknown;
                case Vrm0BlendShapePreset.Neutral:
                    return BlendShapePreset.Neutral;
                case Vrm0BlendShapePreset.A:
                    return BlendShapePreset.A;
                case Vrm0BlendShapePreset.I:
                    return BlendShapePreset.I;
                case Vrm0BlendShapePreset.U:
                    return BlendShapePreset.U;
                case Vrm0BlendShapePreset.E:
                    return BlendShapePreset.E;
                case Vrm0BlendShapePreset.O:
                    return BlendShapePreset.O;
                case Vrm0BlendShapePreset.Blink:
                    return BlendShapePreset.Blink;
                case Vrm0BlendShapePreset.Joy:
                    return BlendShapePreset.Joy;
                case Vrm0BlendShapePreset.Angry:
                    return BlendShapePreset.Angry;
                case Vrm0BlendShapePreset.Sorrow:
                    return BlendShapePreset.Sorrow;
                case Vrm0BlendShapePreset.Fun:
                    return BlendShapePreset.Fun;
                case Vrm0BlendShapePreset.LookUp:
                    return BlendShapePreset.LookUp;
                case Vrm0BlendShapePreset.LookDown:
                    return BlendShapePreset.LookDown;
                case Vrm0BlendShapePreset.LookLeft:
                    return BlendShapePreset.LookLeft;
                case Vrm0BlendShapePreset.LookRight:
                    return BlendShapePreset.LookRight;
                case Vrm0BlendShapePreset.BlinkL:
                    return BlendShapePreset.Blink_L;
                case Vrm0BlendShapePreset.BlinkR:
                    return BlendShapePreset.Blink_R;
                default:
                    // throw new ArgumentOutOfRangeException(nameof(vrm0BlendShape), vrm0BlendShape, null); 
                    return BlendShapePreset.Unknown;
            }
        }

    }
}

#endif