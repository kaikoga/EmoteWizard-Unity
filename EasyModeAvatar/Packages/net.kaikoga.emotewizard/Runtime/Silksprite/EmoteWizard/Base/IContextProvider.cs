using Silksprite.EmoteWizard.Contexts;

namespace Silksprite.EmoteWizard.Base
{
    public interface IContextProvider
    {
        IBehaviourContext ToContext();
    }
}