using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Sequence
{
    public class StaticEmoteFactory : IEmoteFactoryTemplate
    {
        readonly EmoteSequence _sequence;

        public StaticEmoteFactory(EmoteSequence sequence) => _sequence = sequence;

        LayerKind IEmoteFactory.LayerKind => _sequence.layerKind;
        string IEmoteFactory.GroupName => _sequence.groupName;
        bool IEmoteFactory.LooksLikeMirrorItem => _sequence.LooksLikeMirrorItem;
        bool IEmoteFactory.LooksLikeToggle => !_sequence.hasExitTime;

        IEnumerable<Motion> IEmoteFactory.AllClipRefs()
        {
            if (_sequence.clip) yield return _sequence.clip;
            if (_sequence.entryClip) yield return _sequence.entryClip;
            if (_sequence.exitClip) yield return _sequence.exitClip;

        }

        IEnumerable<TrackingOverride> IEmoteFactory.TrackingOverrides()
        {
            return _sequence.hasTrackingOverrides ? _sequence.trackingOverrides : Enumerable.Empty<TrackingOverride>();
        }

        EmoteSequence IEmoteFactory.Build(IEmoteFactory.IClipBuilder builder) => _sequence;

        EmoteSequenceSourceBase IEmoteFactoryTemplate.AddSequenceSource(Component target)
        {
            var source = target.gameObject.AddComponent<EmoteSequenceSource>();
            source.sequence = _sequence;
            return source;
        }
    }
}