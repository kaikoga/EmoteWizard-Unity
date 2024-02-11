using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl.Builders
{
    public interface IEmoteSequenceBuilder
    {
        void AddPath(string path);
        void AddFixedDuration(bool isFixedDuration);
        void AddClip(Motion clip, float entryTransitionDuration = 0.25f, float exitTransitionDuration = 0.25f);
        void AddClipExitTime(bool hasExitTime, float clipExitTime);
        void AddTimeParameter(bool hasTimeParameter, string timeParameter);
        void AddExitClip(bool hasExitClip, Motion exitClip, float exitClipExitTime, float postExitTransitionDuration);
        void AddLayerBlend(bool hasLayerBlend, float blendIn, float blendOut);
        void AddTrackingOverrides(bool hasTrackingOverrides, IEnumerable<TrackingOverride> trackingOverrides);

        IEmoteSequenceFactoryTemplate ToEmoteSequenceFactory();
    }
}