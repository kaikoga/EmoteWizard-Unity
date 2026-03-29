using Silksprite.EmoteWizard.Platforms;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteConditionInstance
    {
        public readonly ParameterItemKind Kind;
        public readonly string Parameter;
        public readonly EmoteConditionMode Mode;
        public readonly ParameterValue Value;

        public EmoteConditionInstance(ParameterItemKind kind, string parameter, EmoteConditionMode mode, ParameterValue value)
        {
            Kind = kind;
            Parameter = parameter;
            Mode = mode;
            Value = value;
        }

        public EmoteConditionInstance ResolveMirror(IPlatformFeatures platformFeatures, EmoteHand hand)
        {
            return new EmoteConditionInstance(Kind, platformFeatures.ResolveMirrorParameter(Parameter, hand), Mode, Value);
        }
    }
}
