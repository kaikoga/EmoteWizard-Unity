using Silksprite.EmoteWizardSupport.Undoable;
using VRM;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class EmoteWizardEnvironmentExtension
    {
        public static void BuildVrm0Avatar(this EmoteWizardEnvironment environment, IUndoable undoable, bool manualBuild)
        {
            var vrmMeta = environment.AvatarRoot.GetComponent<VRMMeta>();
        }
    }
}