using EmoteWizard.Base.DrawerContexts;

namespace EmoteWizard.DataObjects.DrawerContexts
{
    public class ExpressionItemDrawerContext : DrawerContextBase<ExpressionItemDrawerContext>
    {
        public ExpressionItemDrawerContext() : base(null) { }
        public ExpressionItemDrawerContext(EmoteWizardRoot emoteWizardRoot) : base(emoteWizardRoot) { }
    }
}