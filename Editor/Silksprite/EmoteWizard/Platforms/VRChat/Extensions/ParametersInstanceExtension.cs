using System;
using Silksprite.EmoteWizard.DataObjects.Internal;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Extensions
{
    public static class ParametersInstanceExtension
    {
        static VRCExpressionParameters.ValueType GetVrcValueType(this ParameterInstance parameter)
        {
            return parameter.ValueKind switch
            {
                ParameterValueKind.Bool => VRCExpressionParameters.ValueType.Bool,
                ParameterValueKind.Int => VRCExpressionParameters.ValueType.Int,
                ParameterValueKind.Float => VRCExpressionParameters.ValueType.Float,
                ParameterValueKind.HandSign => VRCExpressionParameters.ValueType.Int,
                _ => throw new ArgumentOutOfRangeException()
            };
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