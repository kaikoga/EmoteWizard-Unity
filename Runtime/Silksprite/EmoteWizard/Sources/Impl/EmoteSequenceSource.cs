using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class EmoteSequenceSource : EmoteSequenceSourceBase
    {
        [SerializeField] public EmoteSequence sequence;

        public override bool LooksLikeMirrorItem => sequence != null && sequence.LooksLikeMirrorItem;

        public override IEmoteFactory ToEmoteFactory() => new StaticEmoteFactory(sequence);
    }
}