using System;
using Silksprite.EmoteWizard.DataObjects.Internal;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ParametersInstanceExtension
    {
        static VRCExpressionParameters.ValueType VrcValueType(this ParameterInstance parameter)
        {
            switch (parameter.ValueKind)
            {
                case ParameterValueKind.Bool:
                    return VRCExpressionParameters.ValueType.Bool;
                case ParameterValueKind.Int:
                    return VRCExpressionParameters.ValueType.Int;
                case ParameterValueKind.Float:
                    return VRCExpressionParameters.ValueType.Float;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static VRCExpressionParameters.Parameter ToParameter(this ParameterInstance parameter)
        {
            return new VRCExpressionParameters.Parameter
            {
                name = parameter.Name,
                saved = parameter.Saved,
                defaultValue = parameter.DefaultValue,
                valueType = parameter.VrcValueType(),
                networkSynced = parameter.Synced
            };
        }
    }
}