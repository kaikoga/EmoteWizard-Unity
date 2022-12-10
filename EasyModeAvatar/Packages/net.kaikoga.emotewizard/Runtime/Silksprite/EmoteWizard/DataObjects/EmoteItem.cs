using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Utils;

namespace Silksprite.EmoteWizard.DataObjects
{
    public class EmoteItem
    {
        public readonly EmoteTrigger Trigger;
        public readonly EmoteSequence Sequence;

        public string Group => Sequence.groupName;

        public EmoteItem(EmoteTrigger trigger, EmoteSequence sequence)
        {
            Trigger = trigger;
            Sequence = sequence;
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
                foreach (var condition in Trigger.conditions)
                {
                    if (IsMirrorParameter(condition.parameter)) return true;
                }
                if (Sequence.hasTimeParameter && IsMirrorParameter(Sequence.timeParameter)) return true;

                return false;
            }
        }

        public EmoteInstance Mirror(EmoteHand handValue)
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
                SerializableUtils.Clone(Sequence));

            foreach (var condition in item.Trigger.conditions)
            {
                condition.parameter = ResolveMirrorParameter(condition.parameter);
            }
            item.Sequence.timeParameter = ResolveMirrorParameter(item.Sequence.timeParameter);

            return item;
        }

        public EmoteInstance ToEmoteInstance()
        {
            return new EmoteInstance(Trigger, Sequence);
        }
    }
}
