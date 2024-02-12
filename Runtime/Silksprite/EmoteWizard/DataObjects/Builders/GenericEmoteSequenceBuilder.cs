using System.Collections.Generic;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Builders
{
    public class GenericEmoteSequenceBuilder : IEmoteSequenceBuilder
    {
        readonly GenericEmoteSequence _sequence;
        string _path;

        public GenericEmoteSequenceBuilder(GenericEmoteSequence genericEmoteSequence)
        {
            _sequence = genericEmoteSequence;
        }

        public void AddPath(string path)
        {
            _path = path;
        }

        public void AddFixedDuration(bool isFixedDuration)
        {
            _sequence.isFixedDuration = isFixedDuration;
        }

        public void AddClip(Motion clip, float entryTransitionDuration = 0.25f, float exitTransitionDuration = 0.25f)
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

        public void AddExitClip(bool hasExitClip, Motion exitClip, float exitClipExitTime, float postExitTransitionDuration)
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


        public GenericEmoteSequenceFactory ToGenericEmoteSequenceFactory() => new(_sequence, _path);

        public IEmoteSequenceFactoryTemplate ToEmoteSequenceFactory() => ToGenericEmoteSequenceFactory();
    }
}