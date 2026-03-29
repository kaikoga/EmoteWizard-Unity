using System;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Common.Extensions
{
    public static class ParametersInstanceExtension
    {
        public static AnimatorControllerParameterType GetParameterType(this ParameterInstance parameter)
        {
            return parameter.ValueKind switch
            {
                ParameterValueKind.Int => AnimatorControllerParameterType.Int,
                ParameterValueKind.Float => AnimatorControllerParameterType.Float,
                ParameterValueKind.Bool => AnimatorControllerParameterType.Bool,
                ParameterValueKind.HandSign => AnimatorControllerParameterType.Int,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}