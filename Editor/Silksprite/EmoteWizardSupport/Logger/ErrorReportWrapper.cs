using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

#if EW_NDMF
using nadena.dev.ndmf;
#endif

namespace Silksprite.EmoteWizardSupport.Logger
{
    public static class ErrorReportWrapper
    {
        public static void LogWarningFormat(LocalizedContent loc, Substitution substitution)
        {
#if EW_NDMF
            ErrorReport.ReportError(new WrappedError(ErrorSeverity.NonFatal, loc, null, substitution));
#else
            Debug.LogWarningFormat(message, args);
#endif
        }

        public static void LogWarningFormat(LocalizedContent loc, Object target, Substitution substitution)
        {
#if EW_NDMF
            ErrorReport.ReportError(new WrappedError(ErrorSeverity.NonFatal, loc, target, substitution));
#else
            Debug.LogWarningFormat(message, target, args);
#endif
        }
    }
}