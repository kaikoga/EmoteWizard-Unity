using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    [UsedImplicitly]
    public class ActionEmoteDrawerContext : EmoteWizardDrawerContextBase<ActionEmote, ActionEmoteDrawerContext>
    {
        public ActionEmoteDrawerContext() : base(null) { }
        public ActionEmoteDrawerContext(EmoteWizardRoot emoteWizardRoot) : base(emoteWizardRoot)
        {
        }
    }
}