using JetBrains.Annotations;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Builder;
using Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;

namespace Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Builder
{
    public class VRChatAvatarBuilderContext : AvatarBuilderContextBase
    {
        [UsedImplicitly]
        public VRChatAvatarBuilderContext(EmoteWizardEnvironment environment) : base(environment) { }

        public override void CleanupAvatar()
        {
            Environment.CleanupVrcAvatar();
        }

        public override void BuildAvatar(IUndoable undoable, bool manualBuild)
        {
            Environment.BuildVrcAvatar(undoable, manualBuild);
        }
    }
}
