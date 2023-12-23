using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public interface IEmoteFactory
    {
        LayerKind LayerKind { get; }
        string GroupName { get; }
        bool LooksLikeMirrorItem { get; }
        bool LooksLikeToggle { get; }

        IEnumerable<Motion> AllClipRefs();

        EmoteSequence Build(EmoteWizardEnvironment environment);
        IEnumerable<TrackingOverride> TrackingOverrides();
    }

    public class StaticEmoteFactory : IEmoteFactory
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

        EmoteSequence IEmoteFactory.Build(EmoteWizardEnvironment environment) => _sequence;
    }
}