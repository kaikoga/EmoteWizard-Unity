using System.Collections.Generic;
using Silksprite.EmoteWizard.Platforms;

namespace Silksprite.EmoteWizard.Templates
{
    public interface ICompositeEmoteTemplate : IEmoteTemplate
    {
        IEnumerable<IEmoteTemplate> Unpack(IPlatformFeatures platformFeatures);
    }
}