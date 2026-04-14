using Ablet.ErrorReporting;
using Silksprite.Loch;
using UnityEngine;

#if EW_NDMF_SUPPORT
using Ablet.API;
using nadena.dev.ndmf;
using NdmfErrorReport = nadena.dev.ndmf.ErrorReport;
#endif

using AbletErrorReport = Ablet.ErrorReporting.ErrorReport;

namespace Silksprite.EmoteWizardSupport.Logger
{
    public static class ErrorReportWrapper
    {
        public static void LogWarningFormat(LocalizedContent loc)
        {
#if EW_NDMF_SUPPORT
            if (!AbletSymbols.PreferAblet)
            {
                NdmfErrorReport.ReportError(new WrappedError(ErrorSeverity.NonFatal, loc, null));
                return;
            }
#endif
            AbletErrorReport.LogWarning(loc.Tr);
        }

        public static void LogWarningFormat(LocalizedContent loc, Object target)
        {
#if EW_NDMF_SUPPORT
            if (!AbletSymbols.PreferAblet)
            {
                NdmfErrorReport.ReportError(new WrappedError(ErrorSeverity.NonFatal, loc, target));
                return;
            }
#endif
            using var _ = new InterestScope(target);
            AbletErrorReport.LogWarning(loc.Tr);
        }
    }
}