using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Sequence
{
    [AddComponentMenu("Emote Wizard/Sources/Emote Sequence Source", 100)]
    public class EmoteSequenceSource : EmoteSequenceSourceBase
    {
        [SerializeField] public EmoteSequence sequence = new();

        public override IEmoteSequenceFactoryTemplate ToEmoteFactoryTemplate() => new EmoteSequenceFactory(sequence);
    }
}