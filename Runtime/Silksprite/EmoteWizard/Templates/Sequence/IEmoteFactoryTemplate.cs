using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Sequence
{
    public interface IEmoteFactoryTemplate : IEmoteFactory
    {
        EmoteSequenceSourceBase AddSequenceSource(Component target);
    }
}