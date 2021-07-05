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
        ParameterValueKindFlags _valueKindFlags;
        bool saved = true;
        float defaultValue;
        float usageValue;
        bool defaultParameter;
        readonly List<ParameterState> states = new List<ParameterState>();

        ParameterValueKind ConvertValueKind
        {
            get
            {
                if (_valueKindFlags.HasFlag(ParameterValueKindFlags.Bool)) return ParameterValueKind.Bool;
                if (_valueKindFlags.HasFlag(ParameterValueKindFlags.Int)) return ParameterValueKind.Int;
                if (_valueKindFlags.HasFlag(ParameterValueKindFlags.Float)) return ParameterValueKind.Float;
                return ParameterValueKind.Int;
            }
            set
            {
                switch (value)
                {
                    case ParameterValueKind.Int:
                        _valueKindFlags = ParameterValueKindFlags.Int;
                        break;
                    case ParameterValueKind.Float:
                        _valueKindFlags = ParameterValueKindFlags.Float;
                        break;
                    case ParameterValueKind.Bool:
                        _valueKindFlags = ParameterValueKindFlags.Bool | ParameterValueKindFlags.Int | ParameterValueKindFlags.Float;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        VRCExpressionParameters.ValueType ConvertVrcValueType
        {
            set
            {
                switch (value)
                {
                    case VRCExpressionParameters.ValueType.Int:
                        ConvertValueKind = ParameterValueKind.Int; 
                        break;
                    case VRCExpressionParameters.ValueType.Float:
                        ConvertValueKind = ParameterValueKind.Float; 
                        break;
                    case VRCExpressionParameters.ValueType.Bool:
                        ConvertValueKind = ParameterValueKind.Bool; 
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
                _valueKindFlags = ParameterValueKindFlags.Bool | ParameterValueKindFlags.Int | ParameterValueKindFlags.Float,
                saved = false,
                defaultValue = 0,
                usageValue = 0,
                defaultParameter = false
            };
        }

        public void AddUsage(float value)
        {
            const float epsilon = 0.00001f;
            if (usageValue != 0 && Mathf.Abs(usageValue - value) > epsilon)
            {
                _valueKindFlags &= ~ParameterValueKindFlags.Bool;
            }
            if (value > 1)
            {
                _valueKindFlags &= ~ParameterValueKindFlags.Float;
            }
            if (Mathf.Abs(value % 1f) > 0f)
            {
                _valueKindFlags &= ~ParameterValueKindFlags.Int;
            }
            usageValue = value;
            if (states.All(state => state.value != 0))
            {
                states.Add(new ParameterState { value = 0 } );
            }
            states.Add(new ParameterState { value = value } );
        }

        public void AddPuppetUsage()
        {
            _valueKindFlags &= ~ParameterValueKindFlags.Bool;
            _valueKindFlags &= ~ParameterValueKindFlags.Int;
            usageValue = 0.5f;
        }

        public void Import(VRCExpressionParameters.Parameter parameter)
        {
            name = parameter.name;
            saved = parameter.saved;
            defaultValue = parameter.defaultValue;
            ConvertVrcValueType = parameter.valueType;
            usageValue = 0f;
        }

        public void Import(ParameterItem parameter)
        {
            name = parameter.name;
            saved = parameter.saved;
            defaultValue = parameter.defaultValue;
            ConvertValueKind = parameter.valueKind;
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
                valueKind = ConvertValueKind,
                defaultParameter = defaultParameter,
                states = states.Where(_ => !defaultParameter)
                    .GroupBy(state => state.value)
                    .Select(group => new ParameterState
                    {
                        value = group.Key,
                    })
                    .OrderBy(state => state.value)
                    .ToList()
            };
        }
    }
}