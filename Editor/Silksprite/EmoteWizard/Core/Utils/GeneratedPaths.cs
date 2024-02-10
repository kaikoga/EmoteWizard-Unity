using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Utils
{
    public static class GeneratedPaths
    {
        public static readonly GeneratedPath Temporary = new GeneratedPath("Temporary");

        public static readonly GeneratedPath GeneratedEmpty = new GeneratedPath("@@@Generated@@@Empty.anim");

        public static readonly GeneratedPath GestureDefaultMask = new GeneratedPath("Gesture/@@@Generated@@@GestureDefaultMask.mask");
        public static GeneratedPath GeneratedLayer(LayerKind layerKind) => new GeneratedPath($"{layerKind}/@@@Generated@@@{layerKind}.controller");
        public static GeneratedPath GeneratedResetLayer(LayerKind layerKind) => new GeneratedPath($"{layerKind}/@@@Generated@@@Reset{layerKind}.anim");

        public static readonly GeneratedPath GeneratedEditor = new GeneratedPath("Editor/@@@Generated@@@Editor.controller");

        public static readonly GeneratedPath GeneratedExprParams = new GeneratedPath("Expressions/@@@Generated@@@ExprParams.asset");
        public static readonly GeneratedPath GeneratedExprMenu = new GeneratedPath("Expressions/@@@Generated@@@ExprMenu.asset");

    }
}