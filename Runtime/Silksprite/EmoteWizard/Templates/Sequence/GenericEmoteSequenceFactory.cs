using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Sequence
{
    public class GenericEmoteSequenceFactory : IEmoteSequenceFactoryTemplate
    {
        readonly GenericEmoteSequence _genericEmoteSequence;
        readonly string _clipName;

        public GenericEmoteSequenceFactory(GenericEmoteSequence genericEmoteSequence, string clipName)
        {
            _genericEmoteSequence = genericEmoteSequence;
            _clipName = clipName;
        }

        LayerKind IEmoteSequenceFactory.LayerKind => _genericEmoteSequence.layerKind;
        string IEmoteSequenceFactory.GroupName => _genericEmoteSequence.groupName;
        bool IEmoteSequenceFactory.LooksLikeMirrorItem => false;
        bool IEmoteSequenceFactory.LooksLikeToggle => true;

        IEnumerable<Motion> IEmoteSequenceFactory.AllClipRefs()
        {
            return Enumerable.Empty<Motion>();
        }

        IEnumerable<TrackingOverride> IEmoteSequenceFactory.TrackingOverrides()
        {
            return _genericEmoteSequence.hasTrackingOverrides ? _genericEmoteSequence.trackingOverrides : Enumerable.Empty<TrackingOverride>();
        }

        EmoteSequence IEmoteSequenceFactory.Build(IEmoteSequenceFactory.IClipBuilder builder)
        {
            var avatarRootTransform = builder.Environment.AvatarDescriptor.transform;
            var clip = builder.Build(_clipName, _genericEmoteSequence.ToAnimatedFloats(avatarRootTransform));

            return new EmoteSequence
            {
                layerKind = _genericEmoteSequence.layerKind,
                groupName = _genericEmoteSequence.groupName,

                isFixedDuration = _genericEmoteSequence.isFixedDuration,
                clip = clip,
                entryTransitionDuration = _genericEmoteSequence.entryTransitionDuration,
                exitTransitionDuration = _genericEmoteSequence.exitTransitionDuration,

                hasLayerBlend = _genericEmoteSequence.hasLayerBlend,
                blendIn = _genericEmoteSequence.blendIn,
                blendOut = _genericEmoteSequence.blendOut,
                    
                hasTrackingOverrides = _genericEmoteSequence.hasTrackingOverrides,
                trackingOverrides = _genericEmoteSequence.trackingOverrides.ToList()
            };
        }
        
        EmoteSequenceSourceBase IEmoteSequenceFactoryTemplate.AddSequenceSource(Component target)
        {
            var source = target.gameObject.AddComponent<GenericEmoteSequenceSource>();
            source.sequence = _genericEmoteSequence;
            return source;
        }
    }
}