using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public struct ParameterWriteUsage
    {
        [SerializeField] public ParameterWriteUsageKind writeUsageKind;
        [SerializeField] float value;
        [SerializeField] public ParameterWriteSourceKind writeSourceKind;

        public ParameterValue Value => ParameterValue.Create(writeUsageKind, value);

        public ParameterWriteUsage(ParameterWriteUsageKind writeUsageKind, float value, ParameterWriteSourceKind writeSourceKind)
        {
            this.writeUsageKind = writeUsageKind;
            this.value = value;
            this.writeSourceKind = writeSourceKind;
        }
    }
}