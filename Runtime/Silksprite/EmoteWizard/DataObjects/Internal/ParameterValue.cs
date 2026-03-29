using System;
using Silksprite.EmoteWizard.Platforms;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public readonly struct ParameterValue : IComparable<ParameterValue>
    {
        public static ParameterValue Default => new ParameterValue(ParameterItemKind.Auto, 0);

        readonly ParameterItemKind _itemKind;
        readonly float _value;

        public ParameterItemKind ItemKind => _itemKind;
        public bool IsDefault => _value == 0;

        public int AsInt(IPlatformFeatures platformFeatures) => (int)AsFloat(platformFeatures);

        public float AsFloat(IPlatformFeatures platformFeatures) => _itemKind switch
        {
            ParameterItemKind.HandSign => platformFeatures.HandSignValue((HandSign)_value),
            _ => _value
        };

        public HandSign AsHandSign() => (HandSign)_value;

        ParameterValue(ParameterItemKind itemKind, float value)
        {
            _itemKind = itemKind;
            _value = value;
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
            var itemKindComparison = ((int)_itemKind).CompareTo((int)other._itemKind);
            if (itemKindComparison != 0)
            {
                return itemKindComparison;
            }
            return _value.CompareTo(other._value);
        }
    }
}
