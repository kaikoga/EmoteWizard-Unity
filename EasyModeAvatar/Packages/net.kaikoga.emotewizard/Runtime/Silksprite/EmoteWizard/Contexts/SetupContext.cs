using UnityEngine;

namespace Silksprite.EmoteWizard.Contexts
{
    public class SetupContext : IBehaviourContext
    {
        readonly SetupWizard _wizard;

        public SetupContext(SetupWizard wizard) => _wizard = wizard;

        public IEmoteWizardEnvironment Environment => _wizard.Environment;

        public GameObject GameObject => _wizard.gameObject;

        public bool IsSetupMode => _wizard.isSetupMode;

        public void DisconnectOutputAssets() { }
    }
}