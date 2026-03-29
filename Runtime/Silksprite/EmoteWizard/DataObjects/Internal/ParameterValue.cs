using System;
using Silksprite.EmoteWizard.Platforms;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public struct ParameterValue : IComparable<ParameterValue>
    {
        public static ParameterValue Default => new ParameterValue(ParameterItemKind.Auto, 0);

        [SerializeField] ParameterItemKind itemKind;
        [SerializeField] float value;

        public bool IsDefault => value == 0;

        public int AsInt(IPlatformFeatures platformFeatures) => (int)AsFloat(platformFeatures);

        public float AsFloat(IPlatformFeatures platformFeatures) => itemKind switch
            {
                ParameterItemKind.HandSign => platformFeatures.HandSignValue((HandSign)value),
                _ => value
            };

        ParameterValue(ParameterItemKind itemKind, float value)
        {
            this.itemKind = itemKind;
            this.value = value;
        }

        public static ParameterValue Create(ParameterItemKind itemKind, float value)
        {
            return new ParameterValue(itemKind, value);
        }

        public static ParameterValue Create(ParameterWriteUsageKind writeUsageKind, float value)
        {
            return new ParameterValue(writeUsageKind switch
            {
                ParameterWriteUsageKind.Default => ParameterItemKind.Auto,
                ParameterWriteUsageKind.Auto => ParameterItemKind.Auto,
                ParameterWriteUsageKind.Bool => ParameterItemKind.Bool,
                ParameterWriteUsageKind.Int => ParameterItemKind.Int,
                ParameterWriteUsageKind.Float => ParameterItemKind.Float,
                ParameterWriteUsageKind.HandSign => ParameterItemKind.HandSign,
                _ => throw new ArgumentOutOfRangeException()
            }, value);
        }

        public static ParameterValue Create(ParameterValueKind? valueKind, float value)
        {
            return new ParameterValue(valueKind switch {
                null => ParameterItemKind.Auto,
                ParameterValueKind.Bool => ParameterItemKind.Bool,
                ParameterValueKind.Int => ParameterItemKind.Int,
                ParameterValueKind.Float => ParameterItemKind.Float,
                ParameterValueKind.HandSign => ParameterItemKind.HandSign,
                _ => throw new ArgumentOutOfRangeException(nameof(valueKind), valueKind, null)
            }, value);
        }

        public static ParameterValue HandSign(HandSign handSign)
        {
            return new ParameterValue(ParameterItemKind.HandSign, (int)handSign);
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
