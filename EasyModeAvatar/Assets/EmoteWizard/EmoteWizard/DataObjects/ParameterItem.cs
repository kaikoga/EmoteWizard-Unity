using System;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.DataObjects
{
    public class ParameterItem
    {
        public string Name;
        public ParameterValueTypeFlags ValueTypeFlags;
        public bool Saved = true;
        public float DefaultValue;
        float usageValue;
        public bool DefaultParameter;

        public static List<ParameterItem> VrcDefaultParameters =>
            new List<ParameterItem>
            {
                new ParameterItem
                {
                    DefaultValue = 0,
                    Name = "VRCEmote",
                    Saved = false,
                    ValueTypeFlags = ParameterValueTypeFlags.Int,
                    DefaultParameter = true
                },
                new ParameterItem
                {
                    DefaultValue = 0,
                    Name = "VRCFaceBlendH",
                    Saved = false,
                    ValueTypeFlags = ParameterValueTypeFlags.Float,
                    DefaultParameter = true
                },
                new ParameterItem
                {
                    DefaultValue = 0,
                    Name = "VRCFaceBlendV",
                    Saved = false,
                    ValueTypeFlags = ParameterValueTypeFlags.Float,
                    DefaultParameter = true
                }
            };

        public static ParameterItem Populate(string name)
        {
            return new ParameterItem
            {
                Name = name,
                ValueTypeFlags = ParameterValueTypeFlags.Bool | ParameterValueTypeFlags.Int | ParameterValueTypeFlags.Float,
                Saved = false,
                DefaultValue = 0,
                usageValue = 0,
                DefaultParameter = false
            };
        }

        public void AddUsage(float value)
        {
            const float epsilon = 0.00001f;
            if (usageValue != 0 && Mathf.Abs(usageValue - value) > epsilon)
            {
                ValueTypeFlags &= ~ParameterValueTypeFlags.Bool;
            }
            if (value > 1)
            {
                ValueTypeFlags &= ~ParameterValueTypeFlags.Float;
            }
            if (Mathf.Abs(value % 1f) > 0f)
            {
                ValueTypeFlags &= ~ParameterValueTypeFlags.Int;
            }
            usageValue = value;
        }

        public void Import(VRCExpressionParameters.Parameter parameter)
        {
            Name = parameter.name;
            Saved = parameter.saved;
            DefaultValue = parameter.defaultValue;
            switch (parameter.valueType)
            {
                case VRCExpressionParameters.ValueType.Int:
                    ValueTypeFlags = ParameterValueTypeFlags.Int;
                    break;
                case VRCExpressionParameters.ValueType.Float:
                    ValueTypeFlags = ParameterValueTypeFlags.Float;
                    break;
                case VRCExpressionParameters.ValueType.Bool:
                    ValueTypeFlags = ParameterValueTypeFlags.Bool | ParameterValueTypeFlags.Int | ParameterValueTypeFlags.Float;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            usageValue = 0f;
        }

        public void Import(ParameterItem parameter)
        {
            Name = parameter.Name;
            Saved = parameter.Saved;
            DefaultValue = parameter.DefaultValue;
            ValueTypeFlags = parameter.ValueTypeFlags;
            usageValue = 0f;
            DefaultParameter |= parameter.DefaultParameter;
        }

        public VRCExpressionParameters.Parameter Export()
        {
            VRCExpressionParameters.ValueType GuessValueType()
            {
                if (ValueTypeFlags.HasFlag(ParameterValueTypeFlags.Bool)) return VRCExpressionParameters.ValueType.Bool;
                if (ValueTypeFlags.HasFlag(ParameterValueTypeFlags.Int)) return VRCExpressionParameters.ValueType.Int;
                if (ValueTypeFlags.HasFlag(ParameterValueTypeFlags.Float)) return VRCExpressionParameters.ValueType.Float;
                return VRCExpressionParameters.ValueType.Int;
            }

            // Debug.Log($"Name {Name} ValueType {ValueTypes} => {GuessValueType()}");

            return new VRCExpressionParameters.Parameter
            {
                name = Name,
                saved = Saved,
                defaultValue = DefaultValue,
                valueType = GuessValueType()
            };
        }
    }
}