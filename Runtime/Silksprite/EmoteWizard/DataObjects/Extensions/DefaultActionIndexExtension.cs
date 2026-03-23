using System;

namespace Silksprite.EmoteWizard.DataObjects.Extensions
{
    public static class DefaultActionIndexExtension
    {
        public static string Name(this DefaultActionIndex index)
        {
            return index switch
            {
                DefaultActionIndex.Wave => "Wave",
                DefaultActionIndex.Clap => "Clap",
                DefaultActionIndex.Point => "Point",
                DefaultActionIndex.Cheer => "Cheer",
                DefaultActionIndex.Dance => "Dance",
                DefaultActionIndex.Backflip => "Backflip",
                DefaultActionIndex.SadKick => "SadKick",
                DefaultActionIndex.Die => "Die",
                DefaultActionIndex.Afk => "AFK",
                DefaultActionIndex.Emote1 => "Emote1",
                DefaultActionIndex.Emote2 => "Emote2",
                DefaultActionIndex.Emote3 => "Emote3",
                DefaultActionIndex.Emote4 => "Emote4",
                DefaultActionIndex.Emote5 => "Emote5",
                DefaultActionIndex.Emote6 => "Emote6",
                DefaultActionIndex.Emote7 => "Emote7",
                DefaultActionIndex.Emote8 => "Emote8",
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }
    }
}