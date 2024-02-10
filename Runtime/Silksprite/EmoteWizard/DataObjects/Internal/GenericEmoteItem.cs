namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class GenericEmoteItem
    {
        public readonly HandSign HandSign;
        readonly IGenericEmoteSequenceFactory _emoteSequenceFactory;

        public GenericEmoteSequence GenericEmoteSequence => _emoteSequenceFactory.BuildGeneric();

        public GenericEmoteItem(HandSign handSign, IGenericEmoteSequenceFactory sequenceFactory)
        {
            HandSign = handSign;
            _emoteSequenceFactory = sequenceFactory;
        }
    }
}
