using JetBrains.Annotations;

namespace Silksprite.EmoteWizard.Contexts
{
    public class SetupContext : ContextBase<SetupWizard>
    {
        public readonly bool IsSetupMode;

        [UsedImplicitly]
        public SetupContext(EmoteWizardEnvironment env) : base(env) { }
        public SetupContext(EmoteWizardEnvironment env, SetupWizard wizard) : base(env, wizard) => IsSetupMode = Wizard.isSetupMode;

        public override void DisconnectOutputAssets() { }
    }
}