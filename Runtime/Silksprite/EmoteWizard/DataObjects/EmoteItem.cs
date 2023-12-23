using System.Collections.Generic;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    public class EmoteItem
    {
        public readonly EmoteTrigger Trigger;
        readonly IEmoteFactory _emoteFactory;

        public LayerKind LayerKind => _emoteFactory.LayerKind;
        public string GroupName => _emoteFactory.GroupName;

        public IEnumerable<Motion> AllClipRefs() => _emoteFactory.AllClipRefs();
        public IEnumerable<TrackingOverride> TrackingOverrides() => _emoteFactory.TrackingOverrides();

        public EmoteItem(EmoteTrigger trigger, IEmoteFactory factory)
        {
            Trigger = trigger;
            _emoteFactory = factory;
        }

        public bool IsMirrorItem => Trigger.LooksLikeMirrorItem || _emoteFactory.LooksLikeMirrorItem;

        public EmoteInstance Mirror(EmoteWizardEnvironment environment, EmoteHand handValue)
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
                _emoteFactory.Build());

            foreach (var condition in item.Trigger.conditions)
            {
                condition.parameter = ResolveMirrorParameter(condition.parameter);
            }
            item.Sequence.timeParameter = ResolveMirrorParameter(item.Sequence.timeParameter);

            return item;
        }

        public EmoteInstance ToEmoteInstance(EmoteWizardEnvironment environment)
        {
            return new EmoteInstance(Trigger, _emoteFactory.Build());
        }
    }
}
