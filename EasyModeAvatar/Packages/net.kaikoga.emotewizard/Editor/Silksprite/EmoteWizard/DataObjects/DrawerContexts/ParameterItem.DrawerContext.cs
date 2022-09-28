using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    [UsedImplicitly]
    public class ParameterItemDrawerContext : EmoteWizardDrawerContextBase<ParameterItem, ParameterItemDrawerContext>
    {
        public readonly bool IsEditable;

        public ParameterItemDrawerContext() : base(null) { }
        public ParameterItemDrawerContext(EmoteWizardRoot emoteWizardRoot, bool isEditable) : base(emoteWizardRoot)
        {
            IsEditable = isEditable;
        }
    }
}