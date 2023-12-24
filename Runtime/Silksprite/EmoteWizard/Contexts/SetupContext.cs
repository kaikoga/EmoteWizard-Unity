using JetBrains.Annotations;

namespace Silksprite.EmoteWizard.Contexts
{
    public class SetupContext : ContextBase<SetupWizard>
    {
        [UsedImplicitly]
        public SetupContext(EmoteWizardEnvironment env) : base(env) { }
        public SetupContext(EmoteWizardEnvironment env, SetupWizard wizard) : base(env, wizard)
        {
        }

        public override void DisconnectOutputAssets() { }
    }
}