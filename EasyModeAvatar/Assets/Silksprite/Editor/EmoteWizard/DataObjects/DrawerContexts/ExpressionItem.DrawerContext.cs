using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    [UsedImplicitly]
    public class ExpressionItemDrawerContext : EmoteWizardDrawerContextBase<ExpressionItem, ExpressionItemDrawerContext>
    {
        public ExpressionItemDrawerContext() : base(null) { }
        public ExpressionItemDrawerContext(EmoteWizardRoot emoteWizardRoot) : base(emoteWizardRoot) { }
    }
}