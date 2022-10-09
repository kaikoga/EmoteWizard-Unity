using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Legacy;

namespace Silksprite.EmoteWizard.Sources.Legacy.Base
{
    public interface IEmoteSourceBase
    {
        IEnumerable<Emote> Emotes { get; }
        bool AdvancedAnimations { get; }
    }
}