using Silksprite.EmoteWizard.Templates;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardDataSourceBase : EmoteWizardBehaviour
    {
        protected EmoteTemplatePath SelfPath => EmoteTemplatePath.SelfPath(CreateEnv(), this);
    }
}