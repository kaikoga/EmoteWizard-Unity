using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Sequence
{
    [AddComponentMenu("Emote Wizard/Sources/Generic Emote Sequence Source", 101)]
    public class GenericEmoteSequenceSource : EmoteSequenceSourceBase
    {
        [SerializeField] public GenericEmoteSequence sequence = new GenericEmoteSequence();

        public override IEmoteSequenceFactoryTemplate ToEmoteFactoryTemplate() => new GenericEmoteSequenceFactory(sequence, $"{gameObject.name}_{gameObject.GetInstanceID()}");
    }
}