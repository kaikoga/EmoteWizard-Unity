using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Sequence
{
    [AddComponentMenu("Emote Wizard/Sources/Simple Emote Source", 101)]
    public class SimpleEmoteSource : EmoteSequenceSourceBase
    {
        [SerializeField] public SimpleEmote simpleEmote = new SimpleEmote();

        public override IEmoteFactoryTemplate ToEmoteFactoryTemplate() => new SimpleEmoteFactory(simpleEmote, $"{gameObject.name}_{gameObject.GetInstanceID()}");
    }
}