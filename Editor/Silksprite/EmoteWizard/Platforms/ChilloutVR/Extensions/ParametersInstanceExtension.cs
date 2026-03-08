using System;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.ChilloutVR.Extensions
{
    public static class ParametersInstanceExtension
    {
        public static AnimatorControllerParameterType GetParameterType(this ParameterInstance parameter)
        {
            switch (parameter.ValueKind)
            {
                case ParameterValueKind.Int:
                    return AnimatorControllerParameterType.Int;
                case ParameterValueKind.Float:
                    return AnimatorControllerParameterType.Float;
                case ParameterValueKind.Bool:
                    return AnimatorControllerParameterType.Bool;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}