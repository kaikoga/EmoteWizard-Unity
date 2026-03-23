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
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }
    }
}