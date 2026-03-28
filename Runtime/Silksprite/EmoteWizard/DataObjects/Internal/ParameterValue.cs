using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public struct ParameterValue : IComparable<ParameterValue>
    {
        [SerializeField] ParameterItemKind itemKind;
        [SerializeField] float value;

        public bool IsDefault => value == 0;

        public int IntValue => (int)value;
        public float FloatValue => value;

        ParameterValue(ParameterItemKind itemKind, float value)
        {
            this.itemKind = itemKind;
            this.value = value;
        }

        public static ParameterValue CreateInstance(ParameterItemKind itemKind, float value)
        {
            return new ParameterValue(itemKind, value);
        }

        public static ParameterValue CreateInstance(ParameterWriteUsageKind writeUsageKind, float value1)
        {
            return new ParameterValue(writeUsageKind switch
            {
                ParameterWriteUsageKind.Default => ParameterItemKind.Auto,
                ParameterWriteUsageKind.Auto => ParameterItemKind.Auto,
                ParameterWriteUsageKind.Bool => ParameterItemKind.Bool,
                ParameterWriteUsageKind.Int => ParameterItemKind.Int,
                ParameterWriteUsageKind.Float => ParameterItemKind.Float,
                _ => throw new ArgumentOutOfRangeException()
            }, value1);
        }

        public int CompareTo(ParameterValue other)
        {
            var itemKindComparison = ((int)itemKind).CompareTo((int)other.itemKind);
            if (itemKindComparison != 0)
            {
                return itemKindComparison;
            }
            return value.CompareTo(other.value);
        }
    }
}
