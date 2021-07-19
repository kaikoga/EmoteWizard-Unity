using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class ExpressionItemDrawerContext : EmoteWizardDrawerContextBase<ExpressionItem, ExpressionItemDrawerContext>
    {
        public ExpressionItemDrawerContext() : base(null) { }
        public ExpressionItemDrawerContext(EmoteWizardRoot emoteWizardRoot) : base(emoteWizardRoot) { }
    }
}