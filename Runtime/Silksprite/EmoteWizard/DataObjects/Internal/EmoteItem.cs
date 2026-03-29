using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteItem
    {
        public readonly EmoteTriggerInstance Trigger;
        readonly IEmoteSequenceFactory _emoteSequenceFactory;

        public readonly EmoteHand Hand;

        public LayerKind LayerKind => _emoteSequenceFactory.LayerKind;
        public string GroupNameNoMirror => _emoteSequenceFactory.GroupName;

        public IEnumerable<Motion> AllClipRefs() => _emoteSequenceFactory.AllClipRefs();
        public IEnumerable<TrackingOverride> TrackingOverrides() => _emoteSequenceFactory.TrackingOverrides();

        public EmoteItem(EmoteTriggerInstance trigger, IEmoteSequenceFactory sequenceFactory)
        {
            Trigger = trigger;
            _emoteSequenceFactory = sequenceFactory;
            Hand = EmoteHand.Neither;
        }

        EmoteItem(EmoteTriggerInstance trigger, IEmoteSequenceFactory sequenceFactory, EmoteHand hand)
        {
            Trigger = trigger;
            _emoteSequenceFactory = sequenceFactory;
            Hand = hand;
        }

        public bool IsMirrorItem => Trigger.LooksLikeMirrorItem || _emoteSequenceFactory.LooksLikeMirrorItem;

        public IEnumerable<EmoteItem> Mirror()
        {
            if (Hand is EmoteHand.Neither)
            {
                yield return new EmoteItem(Trigger, _emoteSequenceFactory, EmoteHand.Left);
                yield return new EmoteItem(Trigger, _emoteSequenceFactory, EmoteHand.Right);
            }
            else
            {
                yield return this;
            }
        }

        public IEnumerable<EmoteItem> NoMirror()
        {
            yield return this;
        }

        public EmoteInstance ToEmoteInstance(EmoteWizardEnvironment environment, IClipBuilder clipBuilder)
        {
            var platformFeatures = environment.GetPlatformFeatures();
            return new EmoteInstance(
                Trigger.ResolveMirror(platformFeatures, Hand),
                _emoteSequenceFactory.Build(environment, clipBuilder).ResolveMirrorInplace(platformFeatures, Hand),
                Hand);
        }
    }
}
