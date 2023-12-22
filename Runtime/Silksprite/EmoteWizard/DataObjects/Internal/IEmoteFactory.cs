using Silksprite.EmoteWizard.Contexts;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public interface IEmoteFactory
    {
        EmoteSequence Build(EmoteWizardEnvironment environment);
    }

    public class StaticEmoteFactory : IEmoteFactory
    {
        readonly EmoteSequence _sequence;

        public StaticEmoteFactory(EmoteSequence sequence) => _sequence = sequence;

        public EmoteSequence Build(EmoteWizardEnvironment environment) => _sequence;
    }
}