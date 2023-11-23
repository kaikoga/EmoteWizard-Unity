namespace Silksprite.EmoteWizard.Contexts
{
    public class SetupContext : ContextBase<SetupWizard>
    {
        public readonly bool IsSetupMode;

        public SetupContext(SetupWizard wizard) : base(wizard) => IsSetupMode = Wizard.isSetupMode;

        public override void DisconnectOutputAssets() { }
    }
}