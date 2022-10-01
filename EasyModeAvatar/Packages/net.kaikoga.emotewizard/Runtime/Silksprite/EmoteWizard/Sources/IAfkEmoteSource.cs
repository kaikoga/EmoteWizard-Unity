using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Sources
{
    public interface IAfkEmoteSource
    {
        IEnumerable<ActionEmote> AfkEmotes { get; }
    }
}