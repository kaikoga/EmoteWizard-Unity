using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Logger;
using Silksprite.EmoteWizardSupport.Utils;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ParametersSnapshotExtension
    {
        // TODO: could this be error?  
        public static ParameterInstance ResolveParameterWithWarning(this ParametersSnapshot snapshot, string parameterName)
        {
            var result = snapshot.ResolveParameter(parameterName);
            if (result == null)
            {
                ErrorReportWrapper.LogWarningFormat(Loc("Warn::Parameter::NotFound."), new Substitution
                {
                    ["parameterName"] = parameterName 
                });
            }
            return result;
        }

        // TODO: could this be error?  
        public static ParameterValueKind? ResolveParameterTypeWithWarning(this ParametersSnapshot snapshot, string parameterName, ParameterItemKind itemKind)
        {
            var result = snapshot.ResolveParameterType(parameterName, itemKind, out var mismatch);

            if (result == null)
            {
                ErrorReportWrapper.LogWarningFormat(Loc("Warn::Parameter::NotFound."), new Substitution
                {
                    ["parameterName"] = parameterName 
                });
            }
            else if (mismatch)
            {
                ErrorReportWrapper.LogWarningFormat(Loc("Warn::Parameter::TypeMismatch."),
                    new Substitution
                    {
                        ["parameterName"] = parameterName,
                        ["itemKind"] = $"{itemKind}",
                        ["resolvedItemKind"] = $"{result}"
                    });
            }
            return result;
        }

    }
}