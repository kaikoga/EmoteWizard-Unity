using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Platforms;
using Silksprite.EmoteWizard.DataObjects.Platforms.Extensions;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using Silksprite.EmoteWizardSupport.Utils;
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
            string ResolveMirrorParameter(string parameter)
            {
                var platformFeatures = environment.GetPlatformFeatures();
                switch (parameter)
                {
                    case EmoteWizardConstants.Params.Gesture:
                        return Hand == EmoteHand.Left ? platformFeatures.GestureLeft : platformFeatures.GestureRight;
                    case EmoteWizardConstants.Params.GestureOther:
                        return Hand == EmoteHand.Left ? platformFeatures.GestureRight : platformFeatures.GestureLeft;
                    case EmoteWizardConstants.Params.GestureWeight:
                        return Hand == EmoteHand.Left ? platformFeatures.GestureLeftWeight : platformFeatures.GestureRightWeight;
                    case EmoteWizardConstants.Params.GestureOtherWeight:
                        return Hand == EmoteHand.Left ? platformFeatures.GestureRightWeight : platformFeatures.GestureLeftWeight;
                    default:
                        return parameter;
                }
            }

            var instance = new EmoteInstance(new EmoteTriggerInstance(Trigger), _emoteSequenceFactory.Build(environment, clipBuilder))
            {
                Hand = Hand
            };

            switch (Hand)
            {
                case EmoteHand.Neither:
                    break;
                case EmoteHand.Left:
                case EmoteHand.Right:
                    instance.Sequence.groupName = $"{GroupNameNoMirror} ({Hand})";
                    foreach (var condition in instance.Trigger.Conditions)
                    {
                        condition.Parameter = ResolveMirrorParameter(condition.Parameter);
                    }
                    instance.Sequence.timeParameter = ResolveMirrorParameter(instance.Sequence.timeParameter);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return instance;
        }
    }
}
