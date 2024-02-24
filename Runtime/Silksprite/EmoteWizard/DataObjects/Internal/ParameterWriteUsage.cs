using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public struct ParameterWriteUsage : IEquatable<ParameterWriteUsage>
    {
        [SerializeField] public ParameterWriteUsageKind writeUsageKind;
        [SerializeField] public float value;

        public ParameterWriteUsage(ParameterWriteUsageKind writeUsageKind, float value)
        {
            this.writeUsageKind = writeUsageKind;
            this.value = value;
        }

        public bool Equals(ParameterWriteUsage other)
        {
            return writeUsageKind == other.writeUsageKind && value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            return obj is ParameterWriteUsage other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)writeUsageKind, value);
        }

        public static bool operator ==(ParameterWriteUsage left, ParameterWriteUsage right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ParameterWriteUsage left, ParameterWriteUsage right)
        {
            return !left.Equals(right);
        }
    }
}