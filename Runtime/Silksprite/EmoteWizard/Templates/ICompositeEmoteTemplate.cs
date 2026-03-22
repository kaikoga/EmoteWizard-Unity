using System.Collections.Generic;
using Silksprite.EmoteWizard.Platforms;

namespace Silksprite.EmoteWizard.Templates
{
    public interface ICompositeEmoteTemplate : IEmoteTemplate
    {
        // NOTE: this MUST NOT return itself
        IEnumerable<IEmoteTemplate> Unpack(IPlatformFeatures platformFeatures);
    }
}