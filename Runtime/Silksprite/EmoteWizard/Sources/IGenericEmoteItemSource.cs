using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.Sources
{
    public interface IGenericEmoteItemSource
    {
        IEnumerable<GenericEmoteItem> ToGenericEmoteItems();
    }
}