using JetBrains.Annotations;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Builder;
using Silksprite.EmoteWizard.Platforms.VRM0.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;

namespace Silksprite.EmoteWizard.Platforms.VRM0.Contexts.Builder
{
    public class VRM0AvatarBuilderContext : AvatarBuilderContextBase
    {
        [UsedImplicitly]
        public VRM0AvatarBuilderContext(EmoteWizardEnvironment environment) : base(environment) { }

        public override void CleanupAvatar()
        {
        }

        public override void BuildAvatar(IUndoable undoable, bool manualBuild)
        {
            Environment.BuildVrm0Avatar(undoable, manualBuild);
        }
    }
}
