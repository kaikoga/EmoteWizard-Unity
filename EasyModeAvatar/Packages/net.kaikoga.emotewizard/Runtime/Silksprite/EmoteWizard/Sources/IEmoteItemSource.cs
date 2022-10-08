using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Sources
{
    public interface IEmoteItemSource
    {
        IEnumerable<EmoteItem> EmoteItems { get; }
    }
}