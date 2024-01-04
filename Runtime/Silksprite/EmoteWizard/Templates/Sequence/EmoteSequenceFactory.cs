using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Sequence
{
    public class EmoteSequenceFactory : IEmoteSequenceFactoryTemplate
    {
        readonly EmoteSequence _sequence;

        public EmoteSequenceFactory(EmoteSequence sequence) => _sequence = sequence;

        LayerKind IEmoteSequenceFactory.LayerKind => _sequence.layerKind;
        string IEmoteSequenceFactory.GroupName => _sequence.groupName;
        bool IEmoteSequenceFactory.LooksLikeMirrorItem => _sequence.LooksLikeMirrorItem;
        bool IEmoteSequenceFactory.LooksLikeToggle => !_sequence.hasExitTime;

        IEnumerable<Motion> IEmoteSequenceFactory.AllClipRefs()
        {
            if (_sequence.clip) yield return _sequence.clip;
            if (_sequence.entryClip) yield return _sequence.entryClip;
            if (_sequence.exitClip) yield return _sequence.exitClip;

        }

        IEnumerable<TrackingOverride> IEmoteSequenceFactory.TrackingOverrides()
        {
            return _sequence.hasTrackingOverrides ? _sequence.trackingOverrides : Enumerable.Empty<TrackingOverride>();
        }

        EmoteSequence IEmoteSequenceFactory.Build(EmoteWizardEnvironment environment, IEmoteSequenceFactory.IClipBuilder builder) => _sequence;

        EmoteSequenceSourceBase IEmoteSequenceFactoryTemplate.PopulateSequenceSource(Component target)
        {
            var source = target.gameObject.AddComponent<EmoteSequenceSource>();
            source.sequence = _sequence;
            return source;
        }
    }
}