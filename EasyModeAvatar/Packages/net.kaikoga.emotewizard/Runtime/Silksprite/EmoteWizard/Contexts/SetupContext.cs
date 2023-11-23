using JetBrains.Annotations;

namespace Silksprite.EmoteWizard.Contexts
{
    public class SetupContext : ContextBase<SetupWizard>
    {
        public readonly bool IsSetupMode;

        [UsedImplicitly]
        public SetupContext(EmoteWizardEnvironment env) : base(env) { }
        public SetupContext(SetupWizard wizard) : base(wizard) => IsSetupMode = Wizard.isSetupMode;

        public override void DisconnectOutputAssets() { }
    }
}