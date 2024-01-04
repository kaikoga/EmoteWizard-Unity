using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Sequence
{
    public interface IEmoteSequenceFactoryTemplate : IEmoteSequenceFactory
    {
        EmoteSequenceSourceBase PopulateSequenceSource(Component target);
    }
}