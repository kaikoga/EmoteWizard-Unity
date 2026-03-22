using System.Collections.Generic;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Builders
{
    public class GenericEmoteSequenceBuilder : IEmoteSequenceBuilder
    {
        readonly GenericEmoteSequence _sequence;
        EmoteTemplatePath _path;

        public GenericEmoteSequenceBuilder(GenericEmoteSequence genericEmoteSequence)
        {
            _sequence = genericEmoteSequence;
        }

        public void AddPath(EmoteTemplatePath path)
        {
            _path = path;
        }

        public void AddFixedDuration(bool isFixedDuration)
        {
            _sequence.isFixedDuration = isFixedDuration;
        }

        public void AddClip(Motion? clip, float entryTransitionDuration = 0.25f, float exitTransitionDuration = 0.25f)
        {
            _sequence.entryTransitionDuration = entryTransitionDuration;
            _sequence.exitTransitionDuration = exitTransitionDuration;
        }

        public void AddMirroredClip(Motion? clipLeft, Motion? clipRight, float entryTransitionDuration = 0.25f, float exitTransitionDuration = 0.25f)
        {
            _sequence.entryTransitionDuration = entryTransitionDuration;
            _sequence.exitTransitionDuration = exitTransitionDuration;
        }

        public void AddClipExitTime(bool hasExitTime, float clipExitTime)
        {
        }

        public void AddTimeParameter(bool hasTimeParameter, string timeParameter)
        {
        }

        public void AddExitClip(bool hasExitClip, Motion? exitClip, float exitClipExitTime, float postExitTransitionDuration)
        {
        }

        public void AddLayerBlend(bool hasLayerBlend, float blendIn, float blendOut)
        {
            _sequence.hasLayerBlend = hasLayerBlend;
            _sequence.blendIn = blendIn;
            _sequence.blendOut = blendOut;
        }

        public void AddTrackingOverrides(bool hasTrackingOverrides, IEnumerable<TrackingOverride> trackingOverrides)
        {
            _sequence.hasTrackingOverrides = hasTrackingOverrides;
            _sequence.trackingOverrides.AddRange(trackingOverrides);
        }


        public GenericEmoteSequenceFactory ToGenericEmoteSequenceFactory() => new GenericEmoteSequenceFactory(_sequence, _path.FileName);

        public IEmoteSequenceFactoryTemplate ToEmoteSequenceFactory() => ToGenericEmoteSequenceFactory();
    }
}