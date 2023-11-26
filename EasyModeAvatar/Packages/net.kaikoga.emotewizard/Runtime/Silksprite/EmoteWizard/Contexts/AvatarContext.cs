using JetBrains.Annotations;

namespace Silksprite.EmoteWizard.Contexts
{
    public class AvatarContext : ContextBase<AvatarWizard>
    {
        [UsedImplicitly]
        public AvatarContext(EmoteWizardEnvironment env) : base(env)
        {
        }

        public AvatarContext(EmoteWizardEnvironment env, AvatarWizard wizard) : base(env, wizard)
        {
        }

        public override void DisconnectOutputAssets()
        {
        }
    }
}