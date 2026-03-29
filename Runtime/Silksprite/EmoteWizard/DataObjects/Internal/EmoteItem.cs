using System;
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

            // TODO: use AnimatorState mirror settings 
            Motion? ResolveMirroredMotion(MirroredMotion mirroredMotion, Motion? fallback) =>
                Hand switch
                {
                    EmoteHand.Neither => fallback,
                    EmoteHand.Left => mirroredMotion.clipLeft,
                    EmoteHand.Right => mirroredMotion.clipRight,
                    _ => throw new ArgumentOutOfRangeException()
                };

            var trigger = new EmoteTriggerInstance(Trigger);
            var sequence = _emoteSequenceFactory.Build(environment, clipBuilder);
            var instance = new EmoteInstance(trigger, sequence, Hand);

            switch (Hand)
            {
                case EmoteHand.Neither:
                    break;
                case EmoteHand.Left:
                case EmoteHand.Right:
                    instance.Sequence.groupName = $"{GroupNameNoMirror} ({Hand})";
                    foreach (var condition in instance.Trigger.Conditions)
                    {
                        condition.Parameter = platformFeatures.ResolveMirrorParameter(condition.Parameter, Hand);
                    }
                    instance.Sequence.timeParameter = platformFeatures.ResolveMirrorParameter(instance.Sequence.timeParameter, Hand);
                    if (instance.Sequence.mirroredClip.useMirroredSettings)
                    {
                        instance.Sequence.clip = ResolveMirroredMotion(instance.Sequence.mirroredClip, instance.Sequence.clip);
                    }
                    if (instance.Sequence.mirroredEntryClip.useMirroredSettings)
                    {
                        instance.Sequence.entryClip = ResolveMirroredMotion(instance.Sequence.mirroredEntryClip, instance.Sequence.entryClip);
                    }
                    if (instance.Sequence.mirroredExitClip.useMirroredSettings)
                    {
                        instance.Sequence.exitClip = ResolveMirroredMotion(instance.Sequence.mirroredExitClip, instance.Sequence.exitClip);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return instance;
        }
    }
}
