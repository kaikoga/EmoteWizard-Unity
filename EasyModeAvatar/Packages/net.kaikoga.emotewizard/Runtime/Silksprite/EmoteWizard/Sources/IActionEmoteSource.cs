using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Sources
{
    public interface IActionEmoteSource
    {
        IEnumerable<ActionEmote> ActionEmotes { get; }
    }
}