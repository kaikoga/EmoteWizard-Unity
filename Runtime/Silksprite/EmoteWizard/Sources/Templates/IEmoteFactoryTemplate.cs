using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Templates
{
    public interface IEmoteFactoryTemplate
    {
        IEmoteFactory ToEmoteFactory();

        EmoteSequenceSourceBase AddSequenceSource(Component target);
    }
}