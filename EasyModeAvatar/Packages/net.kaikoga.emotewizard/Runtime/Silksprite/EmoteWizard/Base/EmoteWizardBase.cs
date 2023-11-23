using Silksprite.EmoteWizard.Contexts;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBase : EmoteWizardBehaviour, IContextProvider
    {
        public abstract IBehaviourContext ToContext(EmoteWizardEnvironment env);
    }
}