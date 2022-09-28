using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;

namespace Silksprite.EmoteWizard.Sources.Extensions
{
    public static class EmoteSourceBaseExtension
    {
        public static void RepopulateDefaultEmotes(this EmoteSourceBase emoteSourceBase)
        {
            var newEmotes = Emote.HandSigns
                .Select(Emote.Populate)
                .ToList();
            emoteSourceBase.emotes = newEmotes;
        }

        public static void RepopulateDefaultEmotes14(this EmoteSourceBase emoteSourceBase)
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
            emoteSourceBase.emotes = newEmotes;
        }
    }
}