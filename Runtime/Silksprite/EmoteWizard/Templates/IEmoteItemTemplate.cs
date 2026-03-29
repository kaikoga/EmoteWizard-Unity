using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms;

namespace Silksprite.EmoteWizard.Templates
{
    public interface IEmoteItemTemplate : IEmoteTemplate
    {
        IEnumerable<EmoteItem> ToEmoteItems(IPlatformFeatures platformFeatures);
    }
}