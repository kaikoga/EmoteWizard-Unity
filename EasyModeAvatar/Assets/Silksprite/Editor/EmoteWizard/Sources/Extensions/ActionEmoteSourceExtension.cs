using Silksprite.EmoteWizard.Internal;

namespace Silksprite.EmoteWizard.Sources.Extensions
{
    public static class ActionEmoteSourceExtension
    {
        public static void RepopulateDefaultActionEmotes(this ActionEmoteSource actionEmoteSource)
        {
            actionEmoteSource.actionEmotes = DefaultActionEmote.PopulateDefaultActionEmotes();
        }
    }
}