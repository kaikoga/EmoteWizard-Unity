namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParameterWriteUsage
    {
        public readonly ParameterWriteUsageKind WriteUsageKind;
        public readonly float Value;

        public ParameterWriteUsage(ParameterWriteUsageKind writeUsageKind, float value)
        {
            WriteUsageKind = writeUsageKind;
            Value = value;
        }
    }
}