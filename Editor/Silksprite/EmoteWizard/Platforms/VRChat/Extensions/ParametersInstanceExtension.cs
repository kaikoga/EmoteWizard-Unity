using System;
using Silksprite.EmoteWizard.DataObjects.Internal;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Extensions
{
    public static class ParametersInstanceExtension
    {
        static VRCExpressionParameters.ValueType GetVrcValueType(this ParameterInstance parameter)
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
                name = parameter.name,
                saved = parameter.saved,
                defaultValue = parameter.defaultValue,
                valueType = parameter.GetVrcValueType(),
                networkSynced = parameter.synced
            };
        }
    }
}