using System;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterItem
    {
        public string name;
        public ParameterValueTypeFlags valueTypeFlags;
        public bool saved = true;
        public float defaultValue;
        float usageValue;
        public bool defaultParameter;

        public static List<ParameterItem> VrcDefaultParameters =>
            new List<ParameterItem>
            {
                new ParameterItem
                {
                    defaultValue = 0,
                    name = "VRCEmote",
                    saved = false,
                    valueTypeFlags = ParameterValueTypeFlags.Int,
                    defaultParameter = true
                },
                new ParameterItem
                {
                    defaultValue = 0,
                    name = "VRCFaceBlendH",
                    saved = false,
                    valueTypeFlags = ParameterValueTypeFlags.Float,
                    defaultParameter = true
                },
                new ParameterItem
                {
                    defaultValue = 0,
                    name = "VRCFaceBlendV",
                    saved = false,
                    valueTypeFlags = ParameterValueTypeFlags.Float,
                    defaultParameter = true
                }
            };

        public static ParameterItem Populate(string name)
        {
            return new ParameterItem
            {
                name = name,
                valueTypeFlags = ParameterValueTypeFlags.Bool | ParameterValueTypeFlags.Int | ParameterValueTypeFlags.Float,
                saved = false,
                defaultValue = 0,
                usageValue = 0,
                defaultParameter = false
            };
        }

        public VRCExpressionParameters.ValueType GuessValueType()
        {
            if (valueTypeFlags.HasFlag(ParameterValueTypeFlags.Bool)) return VRCExpressionParameters.ValueType.Bool;
            if (valueTypeFlags.HasFlag(ParameterValueTypeFlags.Int)) return VRCExpressionParameters.ValueType.Int;
            if (valueTypeFlags.HasFlag(ParameterValueTypeFlags.Float)) return VRCExpressionParameters.ValueType.Float;
            return VRCExpressionParameters.ValueType.Int;
        }

        public void AddUsage(float value)
        {
            const float epsilon = 0.00001f;
            if (usageValue != 0 && Mathf.Abs(usageValue - value) > epsilon)
            {
                valueTypeFlags &= ~ParameterValueTypeFlags.Bool;
            }
            if (value > 1)
            {
                valueTypeFlags &= ~ParameterValueTypeFlags.Float;
            }
            if (Mathf.Abs(value % 1f) > 0f)
            {
                valueTypeFlags &= ~ParameterValueTypeFlags.Int;
            }
            usageValue = value;
        }

        public void Import(VRCExpressionParameters.Parameter parameter)
        {
            name = parameter.name;
            saved = parameter.saved;
            defaultValue = parameter.defaultValue;
            switch (parameter.valueType)
            {
                case VRCExpressionParameters.ValueType.Int:
                    valueTypeFlags = ParameterValueTypeFlags.Int;
                    break;
                case VRCExpressionParameters.ValueType.Float:
                    valueTypeFlags = ParameterValueTypeFlags.Float;
                    break;
                case VRCExpressionParameters.ValueType.Bool:
                    valueTypeFlags = ParameterValueTypeFlags.Bool | ParameterValueTypeFlags.Int | ParameterValueTypeFlags.Float;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            usageValue = 0f;
        }

        public void Import(ParameterItem parameter)
        {
            name = parameter.name;
            saved = parameter.saved;
            defaultValue = parameter.defaultValue;
            valueTypeFlags = parameter.valueTypeFlags;
            usageValue = 0f;
            defaultParameter |= parameter.defaultParameter;
        }

        public VRCExpressionParameters.Parameter Export()
        {
            // Debug.Log($"Name {Name} ValueType {ValueTypes} => {GuessValueType()}");

            return new VRCExpressionParameters.Parameter
            {
                name = name,
                saved = saved,
                defaultValue = defaultValue,
                valueType = GuessValueType()
            };
        }
    }
}