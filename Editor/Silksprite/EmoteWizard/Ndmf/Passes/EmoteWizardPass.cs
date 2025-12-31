using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;

namespace Silksprite.EmoteWizard.Ndmf.Passes
{
    class EmoteWizardPass : Pass<EmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
            foreach (var root in buildContext.AvatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
            {
                var env = root.ToEnv();
                env.PersistGeneratedAssets = false;
                env.AvatarRoot = buildContext.AvatarRootTransform;
#if EW_VRCSDK3_AVATARS
                env.BuildVrcAvatar(new EditorUndoable("Build Emote Wizard from ndmf"), false);
#endif
#if EW_UNIVRM_VRM0
                env.BuildVrm0Avatar(new EditorUndoable("Build Emote Wizard from ndmf"), false);
#endif
#if EW_UNIVRM_VRM1
                env.BuildVrm1Avatar(new EditorUndoable("Build Emote Wizard from ndmf"), false);
#endif
            }
        }
    }
}
