using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Legacy;

namespace Silksprite.EmoteWizard.Sources.Legacy.Base
{
    public interface IParameterEmoteSourceBase
    {
        IEnumerable<ParameterEmote> ParameterEmotes { get; }
    }
}