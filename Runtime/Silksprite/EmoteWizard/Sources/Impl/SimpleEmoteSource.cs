using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class SimpleEmoteSource : EmoteSequenceSourceBase
    {
        [SerializeField] public SimpleEmote simpleEmote = new();

        public override bool LooksLikeMirrorItem => false;

        public override IEmoteFactory ToEmoteFactory() => new SimpleEmoteFactory(simpleEmote);

        class SimpleEmoteFactory : IEmoteFactory
        {
            readonly SimpleEmote _simpleEmote;

            public SimpleEmoteFactory(SimpleEmote simpleEmote) => _simpleEmote = simpleEmote;

            public EmoteSequence Build(EmoteWizardEnvironment environment)
            {
                return new EmoteSequence
                {
                    layerKind = _simpleEmote.layerKind,
                    groupName = _simpleEmote.groupName,

                    isFixedDuration = _simpleEmote.isFixedDuration,
                    clip = environment.EmptyClip,
                    entryTransitionDuration = _simpleEmote.entryTransitionDuration,
                    exitTransitionDuration = _simpleEmote.exitTransitionDuration,

                    hasLayerBlend = _simpleEmote.hasLayerBlend,
                    blendIn = _simpleEmote.blendIn,
                    blendOut = _simpleEmote.blendOut,
                    
                    hasTrackingOverrides = _simpleEmote.hasTrackingOverrides,
                    trackingOverrides = _simpleEmote.trackingOverrides.ToList()
                };
            }
        }
    }
}