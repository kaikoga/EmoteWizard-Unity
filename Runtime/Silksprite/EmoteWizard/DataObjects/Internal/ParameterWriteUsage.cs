namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public readonly struct ParameterWriteUsage
    {
        public readonly ParameterWriteUsageKind WriteUsageKind;
        public readonly ParameterValue Value;
        public readonly ParameterWriteSourceKind WriteSourceKind;

        public ParameterWriteUsage(ParameterWriteUsageKind writeUsageKind, float value, ParameterWriteSourceKind writeSourceKind)
        {
            WriteUsageKind = writeUsageKind;
            Value = ParameterValue.Create(WriteUsageKind, value);
            WriteSourceKind = writeSourceKind;
        }
    }
}