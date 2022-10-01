using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Sources.Base
{
    public interface IEmoteSourceBase
    {
        IEnumerable<Emote> Emotes { get; }
    }
}