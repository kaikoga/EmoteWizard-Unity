using System;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteConditionInstance
    {
        public readonly ParameterItemKind Kind;
        public string Parameter;
        public readonly EmoteConditionMode Mode;
        [Obsolete]
        public readonly float Threshold;
        public readonly ParameterValue Value;
        

        public EmoteConditionInstance(ParameterItemKind kind, string parameter, EmoteConditionMode mode, float threshold, ParameterValue value)
        {
            Kind = kind;
            Parameter = parameter;
            Mode = mode;
            Threshold = threshold;
            Value = value;
        }

        public EmoteConditionInstance(EmoteConditionInstance other) : this(other.Kind, other.Parameter, other.Mode, other.Threshold, other.Value) { }
    }
}
