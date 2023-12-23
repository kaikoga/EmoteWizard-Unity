using System.Collections.Generic;
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
        [SerializeField] public SimpleEmote simpleEmote = new SimpleEmote();

        public override bool LooksLikeMirrorItem => false;

        public override IEmoteFactory ToEmoteFactory() => new SimpleEmoteFactory(simpleEmote, $"{gameObject.name}_{gameObject.GetInstanceID()}");

        class SimpleEmoteFactory : IEmoteFactory
        {
            readonly SimpleEmote _simpleEmote;
            readonly string _clipName;

            public SimpleEmoteFactory(SimpleEmote simpleEmote, string clipName)
            {
                _simpleEmote = simpleEmote;
                _clipName = clipName;
            }

            LayerKind IEmoteFactory.LayerKind => _simpleEmote.layerKind;
            string IEmoteFactory.GroupName => _simpleEmote.groupName;
            bool IEmoteFactory.LooksLikeMirrorItem => false;
            bool IEmoteFactory.LooksLikeToggle => true;

            IEnumerable<Motion> IEmoteFactory.AllClipRefs()
            {
                return Enumerable.Empty<Motion>();
            }

            IEnumerable<TrackingOverride> IEmoteFactory.TrackingOverrides()
            {
                return _simpleEmote.hasTrackingOverrides ? _simpleEmote.trackingOverrides : Enumerable.Empty<TrackingOverride>();
            }

            EmoteSequence IEmoteFactory.Build(EmoteWizardEnvironment environment)
            {
                var clip = new AnimationClip();

                return new EmoteSequence
                {
                    layerKind = _simpleEmote.layerKind,
                    groupName = _simpleEmote.groupName,

                    isFixedDuration = _simpleEmote.isFixedDuration,
                    clip = clip,
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