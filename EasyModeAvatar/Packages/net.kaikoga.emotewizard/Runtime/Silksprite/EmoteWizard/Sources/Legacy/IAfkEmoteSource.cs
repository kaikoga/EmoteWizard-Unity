using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Legacy;

namespace Silksprite.EmoteWizard.Sources.Legacy
{
    public interface IAfkEmoteSource
    {
        IEnumerable<ActionEmote> AfkEmotes { get; }
    }
}