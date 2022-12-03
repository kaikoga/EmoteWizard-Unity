using System;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteItem
    {
        [SerializeField] public EmoteHand hand = EmoteHand.Neither;

        [SerializeField] public EmoteTrigger trigger;
        [SerializeField] public EmoteSequence sequence;
        
        public EmoteItem() : this(new EmoteTrigger(), new EmoteSequence()) { }

        public EmoteItem(EmoteTrigger trigger, EmoteSequence sequence)
        {
            this.trigger = trigger;
            this.sequence = sequence;
        }

        public bool IsMirrorItem
        {
            get
            {
                bool IsMirrorParameter(string parameter)
                {
                    switch (parameter)
                    {
                        case EmoteWizardConstants.Params.Gesture:
                        case EmoteWizardConstants.Params.GestureOther:
                        case EmoteWizardConstants.Params.GestureWeight:
                        case EmoteWizardConstants.Params.GestureOtherWeight:
                            return true;
                        default:
                            return false;
                    }
                }
                foreach (var condition in trigger.conditions)
                {
                    if (IsMirrorParameter(condition.parameter)) return true;
                }
                if (IsMirrorParameter(sequence.timeParameter)) return true;

                return false;
            }
        }

        public EmoteItem Mirror(EmoteHand handValue)
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
            var item = SerializableUtils.Clone(this);
            item.hand = handValue;
            item.trigger.groupName = $"{item.trigger.groupName} ({handValue})";
            foreach (var condition in item.trigger.conditions)
            {
                condition.parameter = ResolveMirrorParameter(condition.parameter);
            }
            item.sequence.timeParameter = ResolveMirrorParameter(item.sequence.timeParameter);
            return item;
        }
    }
}
