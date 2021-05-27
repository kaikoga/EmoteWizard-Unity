using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.DataObjects.Internal
{
    public class ExpressionParameterBuilder
    {
        List<ParameterStub> parameters;

        public ExpressionParameterBuilder()
        {
            parameters = new List<ParameterStub>();
        }

        public ParameterStub FindOrCreate(string name)
        {
            var result = parameters.FirstOrDefault(parameter => parameter.Name == name);
            if (result != null) return result;
            result = ParameterStub.Populate(name);
            parameters.Add(result);
            return result;
        }

        public void Import(IEnumerable<VRCExpressionParameters.Parameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                FindOrCreate(parameter.name).Import(parameter);
            }
        }

        public void Import(IEnumerable<ParameterStub> parameters)
        {
            foreach (var parameter in parameters)
            {
                FindOrCreate(parameter.Name).Import(parameter);
            }
        }

        public VRCExpressionParameters.Parameter[] Export(bool customOnly = false) =>
            parameters.Where(parameter => !customOnly || !parameter.DefaultParameter)
                .Select(parameter => parameter.Export())
                .ToArray();

        public class ParameterStub
        {
            public string Name;
            public ValueTypes ValueTypes;
            public bool Saved = true;
            public float DefaultValue;
            float usageValue;
            public bool DefaultParameter;

            public static List<ParameterStub> VrcDefaultParameters =>
                new List<ParameterStub>
                {
                    new ParameterStub
                    {
                        DefaultValue = 0,
                        Name = "VRCEmote",
                        Saved = false,
                        ValueTypes = ValueTypes.Int,
                        DefaultParameter = true
                    },
                    new ParameterStub
                    {
                        DefaultValue = 0,
                        Name = "VRCFaceBlendH",
                        Saved = false,
                        ValueTypes = ValueTypes.Float,
                        DefaultParameter = true
                    },
                    new ParameterStub
                    {
                        DefaultValue = 0,
                        Name = "VRCFaceBlendV",
                        Saved = false,
                        ValueTypes = ValueTypes.Float,
                        DefaultParameter = true
                    }
                };

            public static ParameterStub Populate(string name)
            {
                return new ParameterStub
                {
                    Name = name,
                    ValueTypes = ValueTypes.Bool | ValueTypes.Int | ValueTypes.Float,
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
                    ValueTypes &= ~ValueTypes.Bool;
                }
                if (value > 1)
                {
                    ValueTypes &= ~ValueTypes.Float;
                }
                if (Mathf.Abs(value % 1f) > 0f)
                {
                    ValueTypes &= ~ValueTypes.Int;
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
                        ValueTypes = ValueTypes.Int;
                        break;
                    case VRCExpressionParameters.ValueType.Float:
                        ValueTypes = ValueTypes.Float;
                        break;
                    case VRCExpressionParameters.ValueType.Bool:
                        ValueTypes = ValueTypes.Bool | ValueTypes.Int | ValueTypes.Float;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                usageValue = 0f;
            }

            public void Import(ParameterStub parameter)
            {
                Name = parameter.Name;
                Saved = parameter.Saved;
                DefaultValue = parameter.DefaultValue;
                ValueTypes = parameter.ValueTypes;
                usageValue = 0f;
                DefaultParameter |= parameter.DefaultParameter;
            }

            public VRCExpressionParameters.Parameter Export()
            {
                VRCExpressionParameters.ValueType GuessValueType()
                {
                    if (ValueTypes.HasFlag(ValueTypes.Bool)) return VRCExpressionParameters.ValueType.Bool;
                    if (ValueTypes.HasFlag(ValueTypes.Int)) return VRCExpressionParameters.ValueType.Int;
                    if (ValueTypes.HasFlag(ValueTypes.Float)) return VRCExpressionParameters.ValueType.Float;
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

        [Flags]
        public enum ValueTypes
        {
            Bool = 1,
            Int = 2,
            Float = 4
        }
    }
}