using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Sources.Impl;

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