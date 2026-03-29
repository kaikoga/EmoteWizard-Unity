namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteConditionInstance
    {
        public readonly ParameterItemKind Kind;
        public string Parameter;
        public readonly EmoteConditionMode Mode;
        public readonly ParameterValue Value;

        public EmoteConditionInstance(ParameterItemKind kind, string parameter, EmoteConditionMode mode, ParameterValue value)
        {
            Kind = kind;
            Parameter = parameter;
            Mode = mode;
            Value = value;
        }

        public EmoteConditionInstance(EmoteConditionInstance other) : this(other.Kind, other.Parameter, other.Mode, other.Value) { }
    }
}
