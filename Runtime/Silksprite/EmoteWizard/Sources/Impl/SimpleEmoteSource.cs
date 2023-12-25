using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Impl;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Simple Emote Source", 101)]
    public class SimpleEmoteSource : EmoteSequenceSourceBase
    {
        [SerializeField] public SimpleEmote simpleEmote = new SimpleEmote();

        public override bool LooksLikeMirrorItem => false;

        public override IEmoteFactory ToEmoteFactory() => new SimpleEmoteFactory(simpleEmote, $"{gameObject.name}_{gameObject.GetInstanceID()}");
    }
}