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

        [Obsolete]
        public float ForcedFloatValueUnsafe => value;

        public bool IsDefault => value == 0;

        public ParameterWriteUsage(ParameterWriteUsageKind writeUsageKind, float value, ParameterWriteSourceKind writeSourceKind)
        {
            this.writeUsageKind = writeUsageKind;
            this.value = value;
            this.writeSourceKind = writeSourceKind;
        }
    }
}