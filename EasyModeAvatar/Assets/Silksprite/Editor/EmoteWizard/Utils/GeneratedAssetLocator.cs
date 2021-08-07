namespace Silksprite.EmoteWizard.Utils
{
    public static class GeneratedAssetLocator
    {
        public static string GeneratedOverrideControllerPath(string layer)
        {
            return $"{layer}/@@@Generated@@@Override{layer}.controller";
        }

        public static string EmoteStateClipPath(string layer, string name)
        {
            return $"{layer}/@@@Generated@@@{layer}_{name}.anim";
        }

        public static string EmoteStateClipPath(string layer, string name, string side)
        {
            return $"{layer}/@@@Generated@@@{layer}_{name}_{side}.anim";
        }

        public static string ParameterEmoteBlendTreePath(string layer, string name)
        {
            return $"{layer}/@@@Generated@@@{layer}_{name}_BlendTree.asset";
        }

        public static string ParameterEmoteStateClipPath(string layer, string name, float value)
        {
            return $"{layer}/@@@Generated@@@{layer}_{name}_{value}.anim";
        }

        public static string MixinDirectoryPath(string layer)
        {
            return $"{layer}/Mixin/";
        }
    }
}