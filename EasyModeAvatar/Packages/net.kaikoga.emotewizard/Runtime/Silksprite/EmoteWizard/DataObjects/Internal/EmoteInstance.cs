namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteInstance
    {
        public readonly EmoteTrigger Trigger;
        public readonly EmoteSequence Sequence;

        public EmoteInstance(EmoteTrigger trigger, EmoteSequence sequence)
        {
            Trigger = trigger;
            Sequence = sequence;
        }
    }
}
