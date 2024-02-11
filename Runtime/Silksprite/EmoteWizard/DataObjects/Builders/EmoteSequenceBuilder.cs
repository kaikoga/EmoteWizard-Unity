using System.Collections.Generic;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Builders
{
    public class EmoteSequenceBuilder : IEmoteSequenceBuilder
    {
        readonly EmoteSequence _sequence;

        public EmoteSequenceBuilder(EmoteSequence emoteSequence)
        {
            _sequence = emoteSequence;
        }

        public void AddPath(string path)
        {
        }

        public void AddFixedDuration(bool isFixedDuration)
        {
            _sequence.isFixedDuration = isFixedDuration;
        }

        public void AddClip(Motion clip, float entryTransitionDuration = 0.25f, float exitTransitionDuration = 0.25f)
        {
            _sequence.clip = clip;
            _sequence.entryTransitionDuration = entryTransitionDuration;
            _sequence.exitTransitionDuration = exitTransitionDuration;
        }

        public void AddClipExitTime(bool hasExitTime, float clipExitTime)
        {
            _sequence.hasExitTime = hasExitTime;
            _sequence.clipExitTime = clipExitTime;
        }

        public void AddTimeParameter(bool hasTimeParameter, string timeParameter)
        {
            _sequence.hasTimeParameter = hasTimeParameter;
            _sequence.timeParameter = timeParameter;
        }

        public void AddExitClip(bool hasExitClip, Motion exitClip, float exitClipExitTime, float postExitTransitionDuration)
        {
            _sequence.hasExitClip = hasExitClip;
            _sequence.exitClip = exitClip;
            _sequence.exitClipExitTime = exitClipExitTime;
            _sequence.postExitTransitionDuration = postExitTransitionDuration;
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

        public IEmoteSequenceFactoryTemplate ToEmoteSequenceFactory()
        {
            return new EmoteSequenceFactory(_sequence);
        }
    }
}