using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.DataObjects.Internal
{
    public class ParameterItemBuilder
    {
        public string name;
        ParameterValueTypeFlags valueTypeFlags;
        bool saved = true;
        float defaultValue;
        float usageValue;
        bool defaultParameter;
        readonly List<ParameterState> states = new List<ParameterState>();

        VRCExpressionParameters.ValueType ConvertValueType
        {
            get
            {
                if (valueTypeFlags.HasFlag(ParameterValueTypeFlags.Bool)) return VRCExpressionParameters.ValueType.Bool;
                if (valueTypeFlags.HasFlag(ParameterValueTypeFlags.Int)) return VRCExpressionParameters.ValueType.Int;
                if (valueTypeFlags.HasFlag(ParameterValueTypeFlags.Float)) return VRCExpressionParameters.ValueType.Float;
                return VRCExpressionParameters.ValueType.Int;
            }
            set
            {
                switch (value)
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
            }
        }

        public static ParameterItemBuilder Populate(string name)
        {
            return new ParameterItemBuilder
            {
                name = name,
                valueTypeFlags = ParameterValueTypeFlags.Bool | ParameterValueTypeFlags.Int | ParameterValueTypeFlags.Float,
                saved = false,
                defaultValue = 0,
                usageValue = 0,
                defaultParameter = false,
                states = { new ParameterState {value = 0} }
            };
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
            states.Add(new ParameterState { value = value } );
        }

        public void Import(VRCExpressionParameters.Parameter parameter)
        {
            name = parameter.name;
            saved = parameter.saved;
            defaultValue = parameter.defaultValue;
            ConvertValueType = parameter.valueType;
            usageValue = 0f;
        }

        public void Import(ParameterItem parameter)
        {
            name = parameter.name;
            saved = parameter.saved;
            defaultValue = parameter.defaultValue;
            ConvertValueType = parameter.valueType;
            usageValue = 0f;
            defaultParameter |= parameter.defaultParameter;
            if (parameter.states != null) states.AddRange(parameter.states);
        }

        public ParameterItem Export()
        {
            return new ParameterItem
            {
                name = name,
                saved = saved,
                defaultValue = defaultValue,
                valueType = ConvertValueType,
                defaultParameter = defaultParameter,
                states = states.Where(_ => !defaultParameter)
                    .GroupBy(state => state.value)
                    .Select(group => new ParameterState
                    {
                        value = group.Key,
                        gestureClip = group.Select(state => state.gestureClip).FirstOrDefault(clip => clip != null),
                        fxClip = group.Select(state => state.fxClip).FirstOrDefault(clip => clip != null)
                    })
                    .OrderBy(state => state.value)
                    .ToList()
            };
        }
    }
}