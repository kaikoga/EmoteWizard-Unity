using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class ParameterItemDrawerContext : EmoteWizardDrawerContextBase<ParameterItem, ParameterItemDrawerContext>
    {
        public readonly bool DefaultParameters;

        public ParameterItemDrawerContext() : base(null) { }
        public ParameterItemDrawerContext(EmoteWizardRoot emoteWizardRoot, bool defaultParameters) : base(emoteWizardRoot)
        {
            DefaultParameters = defaultParameters;
        }
    }
}