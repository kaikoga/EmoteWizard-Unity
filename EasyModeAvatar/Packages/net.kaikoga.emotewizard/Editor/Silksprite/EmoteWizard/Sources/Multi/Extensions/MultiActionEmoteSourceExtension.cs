using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Sources.Impl.Multi;

namespace Silksprite.EmoteWizard.Sources.Multi.Extensions
{
    public static class MultiActionEmoteSourceExtension
    {
        public static void RepopulateDefaultActionEmotes(this MultiActionEmoteSource multiActionEmoteSource)
        {
            multiActionEmoteSource.actionEmotes = DefaultActionEmote.PopulateDefaultActionEmotes();
        }
    }
}