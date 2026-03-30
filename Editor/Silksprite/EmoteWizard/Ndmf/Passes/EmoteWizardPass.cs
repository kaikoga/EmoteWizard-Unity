using nadena.dev.ndmf;
using Silksprite.EmoteWizardSupport.Undoable;

#if EW_VRCSDK3_AVATARS
using Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Builder;
#endif

#if ATIV_DETECTED_VRM0
using Silksprite.EmoteWizard.Platforms.VRM0.Contexts.Builder;
#endif

#if ATIV_DETECTED_VRM1
using Silksprite.EmoteWizard.Platforms.VRM1.Contexts.Builder;
#endif

namespace Silksprite.EmoteWizard.Ndmf.Passes
{
    class EmoteWizardPass : Pass<EmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
            var undoable = new EditorUndoable("Build Emote Wizard from NDMF");

            foreach (var root in buildContext.AvatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
            {
                var env = root.ToEnv();
                env.PersistGeneratedAssets = false;
                env.AvatarRoot = buildContext.AvatarRootTransform;
#if EW_VRCSDK3_AVATARS
                env.GetContext<VRChatAvatarBuilderContext>().BuildAvatar(undoable, false);
#endif
#if ATIV_DETECTED_VRM0
                env.GetContext<VRM0AvatarBuilderContext>().BuildAvatar(undoable, false);
#endif
#if ATIV_DETECTED_VRM1
                env.GetContext<VRM1AvatarBuilderContext>().BuildAvatar(undoable, false);
#endif
            }
        }
    }
}
