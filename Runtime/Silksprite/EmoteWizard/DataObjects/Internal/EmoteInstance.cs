namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteInstance
    {
        public readonly EmoteTrigger Trigger;
        public readonly EmoteSequence Sequence;
        public EmoteHand Hand = EmoteHand.Neither;

        public string GroupName => Sequence.groupName;

        public EmoteInstance(EmoteTrigger trigger, EmoteSequence sequence)
        {
            Trigger = trigger;
            Sequence = sequence;
        }
    }
}
