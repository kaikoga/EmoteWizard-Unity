namespace Silksprite.EmoteWizard.Contexts
{
    public class SetupContext : ContextBase<SetupWizard>
    {
        public SetupContext(SetupWizard wizard) : base(wizard) { }

        public bool IsSetupMode => Wizard.isSetupMode;

        public override void DisconnectOutputAssets() { }
    }
}