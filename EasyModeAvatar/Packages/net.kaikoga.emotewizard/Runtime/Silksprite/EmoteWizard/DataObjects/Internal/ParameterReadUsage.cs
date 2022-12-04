namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParameterReadUsage
    {
        public readonly ParameterItemKind ItemKind;
        public readonly float Value;

        public ParameterReadUsage(ParameterItemKind itemKind, float value)
        {
            ItemKind = itemKind;
            Value = value;
        }
    }
}