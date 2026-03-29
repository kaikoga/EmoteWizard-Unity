namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteInstance
    {
        public readonly EmoteTriggerInstance Trigger;
        public readonly EmoteSequence Sequence;
        public readonly EmoteHand Hand;

        public string GroupName => Sequence.groupName;

        public EmoteInstance(EmoteTriggerInstance trigger, EmoteSequence sequence, EmoteHand hand)
        {
            Trigger = trigger;
            Sequence = sequence;
            Hand = hand;
        }
    }
}
