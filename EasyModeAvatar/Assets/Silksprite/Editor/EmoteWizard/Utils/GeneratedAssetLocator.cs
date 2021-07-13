namespace Silksprite.EmoteWizard.Utils
{
    public static class GeneratedAssetLocator
    {
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