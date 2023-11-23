using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public class SetupContext : ISetupWizardContext
    {
        readonly SetupWizard _wizard;

        public SetupContext(SetupWizard wizard) => _wizard = wizard;

        IEmoteWizardEnvironment IBehaviourContext.Environment => _wizard.Environment;

        GameObject IBehaviourContext.GameObject => _wizard.gameObject;

        bool ISetupWizardContext.IsSetupMode => _wizard.isSetupMode;

        void IBehaviourContext.DisconnectOutputAssets() { }
    }
}