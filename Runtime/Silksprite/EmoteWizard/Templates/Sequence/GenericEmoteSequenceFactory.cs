using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.ClipBuilder;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Sequence
{
    public class GenericEmoteSequenceFactory : IEmoteSequenceFactoryTemplate, IGenericEmoteSequenceFactory
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

        EmoteSequence IEmoteSequenceFactory.Build(EmoteWizardEnvironment environment, IClipBuilder builder)
        {
            var avatarRootTransform = environment.AvatarRoot.transform;
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
        
        public GenericEmoteSequence BuildGeneric() => _genericEmoteSequence;

        EmoteSequenceSourceBase IEmoteSequenceFactoryTemplate.PopulateSequenceSource(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<GenericEmoteSequenceSource>(target);
            source.sequence = _genericEmoteSequence;
            return source;
        }
    }
}