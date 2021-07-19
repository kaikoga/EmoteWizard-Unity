using System;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterUsage
    {
        public ParameterUsageKind usageKind;
        public float value;

        public ParameterUsage()
        {
        }

        public ParameterUsage(ParameterUsageKind usageKind, float value)
        {
            this.usageKind = usageKind;
            this.value = value;
        }
    }
}