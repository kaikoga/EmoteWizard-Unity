namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteConditionInstance
    {
        public readonly ParameterItemKind Kind;
        public string Parameter;
        public readonly EmoteConditionMode Mode;
        public readonly float Threshold;

        public EmoteConditionInstance(ParameterItemKind kind, string parameter, EmoteConditionMode mode, float threshold)
        {
            Kind = kind;
            Parameter = parameter;
            Mode = mode;
            Threshold = threshold;
        }
        
        public EmoteConditionInstance(EmoteConditionInstance other) : this(other.Kind, other.Parameter, other.Mode, other.Threshold) { }
    }
}
