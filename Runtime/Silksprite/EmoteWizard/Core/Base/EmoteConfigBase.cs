using Silksprite.EmoteWizard.Contexts;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteConfigBase : EmoteWizardBehaviour, IContextProvider
    {
        public abstract IBehaviourContext ToContext(EmoteWizardEnvironment env);
    }
}