using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.Sources
{
    public interface IEmoteItemSource
    {
        IEnumerable<EmoteItem> ToEmoteItems();
    }
}