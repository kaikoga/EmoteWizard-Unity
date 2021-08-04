using Silksprite.EmoteWizard.Internal;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ActionWizardExtension
    {
        public static void RepopulateDefaultActionEmotes(this ActionWizard actionWizard)
        {
            actionWizard.actionEmotes = DefaultActionEmote.PopulateDefaultActionEmotes();
            actionWizard.afkEmotes = DefaultActionEmote.PopulateDefaultAfkEmotes();
        }
    }
}