using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;

namespace Silksprite.EmoteWizard.Sources.Multi.Extensions
{
    public static class MultiEmoteSourceBaseExtension
    {
        public static void RepopulateDefaultEmotes(this MultiEmoteSourceBase multiEmoteSourceBase)
        {
            var newEmotes = Emote.HandSigns
                .Select(Emote.Populate)
                .ToList();
            multiEmoteSourceBase.emotes = newEmotes;
        }

        public static void RepopulateDefaultEmotes14(this MultiEmoteSourceBase multiEmoteSourceBase)
        {
            var newEmotes = Enumerable.Empty<Emote>()
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther),
                        control = EmoteControl.Populate(handSign)
                    }))
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther, GestureConditionMode.NotEqual),
                        control = EmoteControl.Populate(handSign)
                    }))
                .ToList();
            multiEmoteSourceBase.emotes = newEmotes;
        }
    }
}