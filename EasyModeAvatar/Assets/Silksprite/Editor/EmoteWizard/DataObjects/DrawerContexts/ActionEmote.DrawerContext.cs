using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    [UsedImplicitly]
    public class ActionEmoteDrawerContext : EmoteWizardDrawerContextBase<ActionEmote, ActionEmoteDrawerContext>
    {
        public readonly ActionEmoteDrawerState State;
        public readonly bool FixedTransitionDuration;
        public readonly bool IsDefaultAfk;

        public ActionEmoteDrawerContext() : base(null) { }
        public ActionEmoteDrawerContext(EmoteWizardRoot emoteWizardRoot, ActionEmoteDrawerState state, bool fixedTransitionDuration, bool isDefaultAfk) : base(emoteWizardRoot)
        {
            State = state;
            FixedTransitionDuration = fixedTransitionDuration;
            IsDefaultAfk = isDefaultAfk;
        }
    }
}