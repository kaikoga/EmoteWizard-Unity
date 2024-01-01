using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;
using UnityEngine.Serialization;

namespace Silksprite.EmoteWizard.Sources.Sequence
{
    [AddComponentMenu("Emote Wizard/Sources/Simple Emote Source", 101)]
    public class GenericEmoteSequenceSource : EmoteSequenceSourceBase
    {
        [FormerlySerializedAs("simpleEmote")] [SerializeField] public GenericEmoteSequence sequence = new GenericEmoteSequence();

        public override IEmoteSequenceFactoryTemplate ToEmoteFactoryTemplate() => new GenericEmoteSequenceFactory(sequence, $"{gameObject.name}_{gameObject.GetInstanceID()}");
    }
}