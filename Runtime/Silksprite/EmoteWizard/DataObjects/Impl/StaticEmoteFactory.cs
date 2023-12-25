using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Templates;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Impl
{
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

        IEmoteFactoryTemplate IEmoteFactory.ToTemplate() => new StaticEmoteFactoryTemplate(this);

        EmoteSequence IEmoteFactory.Build(IEmoteFactory.IClipBuilder builder) => _sequence;

        class StaticEmoteFactoryTemplate : IEmoteFactoryTemplate
        {
            readonly StaticEmoteFactory _factory;

            public StaticEmoteFactoryTemplate(StaticEmoteFactory staticEmoteFactory) => _factory = staticEmoteFactory;

            public IEmoteFactory ToEmoteFactory() => _factory;

            public EmoteSequenceSourceBase AddSequenceSource(Component target)
            {
                var source = target.gameObject.AddComponent<EmoteSequenceSource>();
                source.sequence = _factory._sequence;
                return source;
            }
        }
    }
}