using System.Linq;
using nadena.dev.ndmf;
using nadena.dev.ndmf.localization;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Utils;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.Logger
{
    internal class WrappedError : SimpleError
    {
        public override ErrorSeverity Severity { get; }

        readonly LocalizedContent _loc;
        readonly ObjectReference _context;
        readonly Substitution _substitution;

        #region unused ndmf API
        public override Localizer Localizer => null;

        public override string TitleKey => null;

        public override string[] TitleSubst => null;
        public override string[] DetailsSubst => null;
        public override string[] HintSubst => null;
        #endregion

        public WrappedError(ErrorSeverity errorSeverity, LocalizedContent loc, Object context, Substitution substitution)
        {
            Severity = errorSeverity;
            _loc = loc;
            _substitution = substitution ?? Substitution.Empty;
            AddReference(ObjectRegistry.GetReference(context));
        }

        public override string FormatTitle()
        {
            return _loc.TrFormat(_substitution).SplitCompat("\n").FirstOrDefault();
        }

        public override string FormatDetails()
        {
            return _loc.TrFormat(_substitution);
        }

        public override string FormatHint()
        {
            return null;
        }
    }
}