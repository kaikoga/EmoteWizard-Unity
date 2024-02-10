namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public interface IGenericEmoteSequenceFactory : IEmoteSequenceFactory
    {
        GenericEmoteSequence BuildGeneric();
    }
}