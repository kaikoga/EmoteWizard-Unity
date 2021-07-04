namespace EmoteWizard.Utils
{
    public static class GeneratedAssetLocator
    {
        public static string ParameterEmoteStateClipPath(string layer, string name, float value)
        {
            return $"{layer}/@@@Generated@@@{layer}_{name}_{value}.anim";
        }
    }
}