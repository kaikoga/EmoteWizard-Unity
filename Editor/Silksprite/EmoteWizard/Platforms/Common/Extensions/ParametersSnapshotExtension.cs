using System.Diagnostics.CodeAnalysis;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Logger;

namespace Silksprite.EmoteWizard.Platforms.Common.Extensions
{
    public static class ParametersSnapshotExtension
    {
        public static bool TryResolveParameterWithTypeAndWarning(this ParametersSnapshot snapshot, string parameterName, ParameterItemKind itemKind,
            [MaybeNullWhen(false)] out ParameterInstance parameterInstance,
            out ParameterValueKind actualValueKind)
        {
            return snapshot.TryResolveParameterWithType(parameterName, itemKind, out parameterInstance, out actualValueKind, ErrorReportWrapper.LogWarningFormat);
        }
    }
}