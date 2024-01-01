using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    public class EmoteItem
    {
        public readonly EmoteTrigger Trigger;
        readonly IEmoteSequenceFactory _emoteSequenceFactory;

        public LayerKind LayerKind => _emoteSequenceFactory.LayerKind;
        public string GroupName => _emoteSequenceFactory.GroupName;

        public IEnumerable<Motion> AllClipRefs() => _emoteSequenceFactory.AllClipRefs();
        public IEnumerable<TrackingOverride> TrackingOverrides() => _emoteSequenceFactory.TrackingOverrides();

        public EmoteItem(EmoteTrigger trigger, IEmoteSequenceFactory sequenceFactory)
        {
            Trigger = trigger;
            _emoteSequenceFactory = sequenceFactory;
        }

        public bool IsMirrorItem => Trigger.LooksLikeMirrorItem || _emoteSequenceFactory.LooksLikeMirrorItem;

        public EmoteInstance Mirror(IEmoteSequenceFactory.IClipBuilder clipBuilder, EmoteHand handValue)
        {
            string ResolveMirrorParameter(string parameter)
            {
                switch (parameter)
                {
                    case EmoteWizardConstants.Params.Gesture:
                        return handValue == EmoteHand.Left ? "GestureLeft" : "GestureRight";
                    case EmoteWizardConstants.Params.GestureOther:
                        return handValue == EmoteHand.Left ? "GestureRight" : "GestureLeft";
                    case EmoteWizardConstants.Params.GestureWeight:
                        return handValue == EmoteHand.Left ? "GestureLeftWeight" : "GestureRightWeight";
                    case EmoteWizardConstants.Params.GestureOtherWeight:
                        return handValue == EmoteHand.Left ? "GestureRightWeight" : "GestureLeftWeight";
                    default:
                        return parameter;
                }
            }

            var item = new EmoteInstance(SerializableUtils.Clone(Trigger),
                _emoteSequenceFactory.Build(clipBuilder));

            foreach (var condition in item.Trigger.conditions)
            {
                condition.parameter = ResolveMirrorParameter(condition.parameter);
            }
            item.Sequence.timeParameter = ResolveMirrorParameter(item.Sequence.timeParameter);

            return item;
        }

        public EmoteInstance ToEmoteInstance(IEmoteSequenceFactory.IClipBuilder clipBuilder)
        {
            return new EmoteInstance(Trigger, _emoteSequenceFactory.Build(clipBuilder));
        }
    }
}
