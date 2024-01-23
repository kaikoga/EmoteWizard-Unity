using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.ClipBuilder;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    public class EmoteItem
    {
        public readonly EmoteTrigger Trigger;
        readonly IEmoteSequenceFactory _emoteSequenceFactory;

        public EmoteHand Hand = EmoteHand.Neither;

        public LayerKind LayerKind => _emoteSequenceFactory.LayerKind;
        public string GroupName
        {
            get
            {
                switch (Hand)
                {
                    case EmoteHand.Neither:
                        return _emoteSequenceFactory.GroupName;
                    case EmoteHand.Left:
                    case EmoteHand.Right:
                        return $"{_emoteSequenceFactory.GroupName} ({Hand})";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public IEnumerable<Motion> AllClipRefs() => _emoteSequenceFactory.AllClipRefs();
        public IEnumerable<TrackingOverride> TrackingOverrides() => _emoteSequenceFactory.TrackingOverrides();

        public EmoteItem(EmoteTrigger trigger, IEmoteSequenceFactory sequenceFactory)
        {
            Trigger = trigger;
            _emoteSequenceFactory = sequenceFactory;
        }

        public bool IsMirrorItem => Trigger.LooksLikeMirrorItem || _emoteSequenceFactory.LooksLikeMirrorItem;

        public IEnumerable<EmoteItem> Mirror(bool force)
        {
            EmoteItem MirrorSide(EmoteHand handValue)
            {
                var item = new EmoteItem(Trigger, _emoteSequenceFactory)
                {
                    Hand = handValue
                };

                return item;
            }

            if (force || IsMirrorItem)
            {
                yield return MirrorSide(EmoteHand.Left);
                yield return MirrorSide(EmoteHand.Right);
            }
            else
            {
                yield return this;
            }
        }

        public EmoteInstance ToEmoteInstance(EmoteWizardEnvironment environment, IClipBuilder clipBuilder)
        {
            string ResolveMirrorParameter(string parameter)
            {
                switch (parameter)
                {
                    case EmoteWizardConstants.Params.Gesture:
                        return Hand == EmoteHand.Left ? "GestureLeft" : "GestureRight";
                    case EmoteWizardConstants.Params.GestureOther:
                        return Hand == EmoteHand.Left ? "GestureRight" : "GestureLeft";
                    case EmoteWizardConstants.Params.GestureWeight:
                        return Hand == EmoteHand.Left ? "GestureLeftWeight" : "GestureRightWeight";
                    case EmoteWizardConstants.Params.GestureOtherWeight:
                        return Hand == EmoteHand.Left ? "GestureRightWeight" : "GestureLeftWeight";
                    default:
                        return parameter;
                }
            }

            var instance = new EmoteInstance(SerializableUtils.Clone(Trigger), _emoteSequenceFactory.Build(environment, clipBuilder))
            {
                Hand = Hand
            };

            switch (Hand)
            {
                case EmoteHand.Neither:
                    break;
                case EmoteHand.Left:
                case EmoteHand.Right:
                    instance.Sequence.groupName = GroupName;
                    foreach (var condition in instance.Trigger.conditions)
                    {
                        condition.parameter = ResolveMirrorParameter(condition.parameter);
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
