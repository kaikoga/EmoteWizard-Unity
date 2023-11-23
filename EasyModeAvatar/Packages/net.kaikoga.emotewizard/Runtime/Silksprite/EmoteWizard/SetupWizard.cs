using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class SetupWizard : EmoteWizardBase
    {
        public bool isSetupMode = true;

        public override IBehaviourContext ToContext() => new SetupContext(this);

        class SetupContext : ISetupWizardContext
        {
            readonly SetupWizard _wizard;

            public SetupContext(SetupWizard wizard) => _wizard = wizard;

            IEmoteWizardEnvironment IBehaviourContext.Environment => _wizard.Environment;

            GameObject IBehaviourContext.GameObject => _wizard.gameObject;

            bool ISetupWizardContext.IsSetupMode => _wizard.isSetupMode;

            void IBehaviourContext.DisconnectOutputAssets() { }
        }
    }
}