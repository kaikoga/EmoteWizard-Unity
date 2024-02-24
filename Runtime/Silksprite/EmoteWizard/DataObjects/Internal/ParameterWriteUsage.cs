using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public struct ParameterWriteUsage : IEquatable<ParameterWriteUsage>
    {
        [SerializeField] public ParameterWriteUsageKind WriteUsageKind;
        [SerializeField] public float Value;

        public ParameterWriteUsage(ParameterWriteUsageKind writeUsageKind, float value)
        {
            WriteUsageKind = writeUsageKind;
            Value = value;
        }

        public bool Equals(ParameterWriteUsage other)
        {
            return WriteUsageKind == other.WriteUsageKind && Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is ParameterWriteUsage other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)WriteUsageKind, Value);
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