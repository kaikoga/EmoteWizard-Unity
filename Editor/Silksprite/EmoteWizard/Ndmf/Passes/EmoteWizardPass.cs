using System.Collections.Generic;
using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Builder;
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
                foreach (var builder in GuessBuilder(env))
                {
                    builder.BuildAvatar(undoable, false);
                }
            }
        }

        static IEnumerable<AvatarBuilderContextBase> GuessBuilder(EmoteWizardEnvironment env)
        {
#if EW_VRCSDK3_AVATARS
            yield return env.GetContext<VRChatAvatarBuilderContext>();
#endif
#if ATIV_DETECTED_VRM0
            yield return env.GetContext<VRM0AvatarBuilderContext>();
#endif
#if ATIV_DETECTED_VRM1
            yield return env.GetContext<VRM1AvatarBuilderContext>();
#endif
            yield break;
        }
    }
}
