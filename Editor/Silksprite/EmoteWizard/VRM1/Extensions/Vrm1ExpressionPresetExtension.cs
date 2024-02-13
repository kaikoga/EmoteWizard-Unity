#if EW_VRM1

using Silksprite.EmoteWizard.DataObjects;
using UniVRM10;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class Vrm1ExpressionPresetExtension
    {
        public static ExpressionPreset ToExpressionPreset(this Vrm1ExpressionPreset vrm1Expression)
        {
            switch (vrm1Expression)
            {
                case Vrm1ExpressionPreset.Custom:
                    return ExpressionPreset.custom;
                case Vrm1ExpressionPreset.Happy:
                    return ExpressionPreset.happy;
                case Vrm1ExpressionPreset.Angry:
                    return ExpressionPreset.angry;
                case Vrm1ExpressionPreset.Sad:
                    return ExpressionPreset.sad;
                case Vrm1ExpressionPreset.Relaxed:
                    return ExpressionPreset.relaxed;
                case Vrm1ExpressionPreset.Surprised:
                    return ExpressionPreset.surprised;
                case Vrm1ExpressionPreset.Aa:
                    return ExpressionPreset.aa;
                case Vrm1ExpressionPreset.Ih:
                    return ExpressionPreset.ih;
                case Vrm1ExpressionPreset.Ou:
                    return ExpressionPreset.ou;
                case Vrm1ExpressionPreset.Ee:
                    return ExpressionPreset.ee;
                case Vrm1ExpressionPreset.Oh:
                    return ExpressionPreset.oh;
                case Vrm1ExpressionPreset.Blink:
                    return ExpressionPreset.blink;
                case Vrm1ExpressionPreset.BlinkLeft:
                    return ExpressionPreset.blinkLeft;
                case Vrm1ExpressionPreset.BlinkRight:
                    return ExpressionPreset.blinkRight;
                case Vrm1ExpressionPreset.LookUp:
                    return ExpressionPreset.lookUp;
                case Vrm1ExpressionPreset.LookDown:
                    return ExpressionPreset.lookDown;
                case Vrm1ExpressionPreset.LookLeft:
                    return ExpressionPreset.lookLeft;
                case Vrm1ExpressionPreset.LookRight:
                    return ExpressionPreset.lookRight;
                case Vrm1ExpressionPreset.Neutral:
                    return ExpressionPreset.neutral;
                default:
                    // throw new ArgumentOutOfRangeException(nameof(vrm1Expression), vrm1Expression, null);
                    return ExpressionPreset.custom;
            }
        }
    }
}

#endif