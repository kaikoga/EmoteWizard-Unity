using JetBrains.Annotations;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Builder;
using Silksprite.EmoteWizard.Platforms.VRM1.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;

namespace Silksprite.EmoteWizard.Platforms.VRM1.Contexts.Builder
{
    public class VRM1AvatarBuilderContext : AvatarBuilderContextBase
    {
        [UsedImplicitly]
        public VRM1AvatarBuilderContext(EmoteWizardEnvironment environment) : base(environment) { }

        public override void CleanupAvatar()
        {
            // nop
        }

        public override void BuildAvatar(IUndoable undoable, bool manualBuild)
        {
            Environment.BuildVrm1Avatar(undoable, manualBuild);
        }
    }
}
