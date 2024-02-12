namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class GenericEmoteItem
    {
        public readonly GenericEmoteTrigger Trigger;
        readonly IGenericEmoteSequenceFactory _emoteSequenceFactory;

        public GenericEmoteSequence GenericEmoteSequence => _emoteSequenceFactory.BuildGeneric();

        public GenericEmoteItem(GenericEmoteTrigger trigger, IGenericEmoteSequenceFactory sequenceFactory)
        {
            Trigger = trigger;
            _emoteSequenceFactory = sequenceFactory;
        }
    }
}
