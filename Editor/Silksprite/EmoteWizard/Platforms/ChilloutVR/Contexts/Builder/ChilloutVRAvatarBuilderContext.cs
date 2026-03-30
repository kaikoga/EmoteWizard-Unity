using JetBrains.Annotations;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Builder;
using Silksprite.EmoteWizard.Platforms.ChilloutVR.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;

namespace Silksprite.EmoteWizard.Platforms.ChilloutVR.Contexts.Builder
{
    public class ChilloutVRAvatarBuilderContext : AvatarBuilderContextBase
    {
        [UsedImplicitly]
        public ChilloutVRAvatarBuilderContext(EmoteWizardEnvironment environment) : base(environment) { }

        public override void CleanupAvatar()
        {
            Environment.CleanupCvrAvatar();
        }

        public override void BuildAvatar(IUndoable undoable, bool manualBuild)
        {
            Environment.BuildCvrAvatar(undoable, manualBuild);
        }
    }
}
