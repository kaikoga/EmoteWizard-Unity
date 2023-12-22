using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class SimpleEmoteSequenceSource : EmoteSequenceSourceBase
    {
        [SerializeField] public LayerKind layerKind = LayerKind.FX;
        [SerializeField] public string groupName;

        public override bool LooksLikeMirrorItem => false;

        public override IEmoteFactory ToEmoteFactory() => new SimpleEmoteFactory(this);

        class SimpleEmoteFactory : IEmoteFactory
        {
            readonly SimpleEmoteSequenceSource _source;

            public SimpleEmoteFactory(SimpleEmoteSequenceSource source) => _source = source;

            public EmoteSequence Build(EmoteWizardEnvironment environment)
            {
                return new EmoteSequence
                {
                    layerKind = _source.layerKind,
                    groupName = _source.groupName,
                    clip = environment.EmptyClip
                };
            }
        }
    }
}