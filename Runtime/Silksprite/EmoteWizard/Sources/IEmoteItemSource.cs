using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;

namespace Silksprite.EmoteWizard.Sources
{
    public interface IEmoteItemSource
    {
        IEnumerable<EmoteItem> ToEmoteItems(EmoteWizardEnvironment environment);
    }
}