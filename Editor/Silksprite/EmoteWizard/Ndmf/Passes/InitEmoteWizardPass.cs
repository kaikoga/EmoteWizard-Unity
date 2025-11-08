using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;

#if ATIV_DETECTED_VRM0
using VRM;
#endif

#if ATIV_DETECTED_VRM1
using UniVRM10;
#endif

namespace Silksprite.EmoteWizard.Ndmf.Passes
{
    class InitEmoteWizardPass : Pass<InitEmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
            var undoable = new EditorUndoable("Prepare Emote Wizard from ndmf");

            var avatarRootTransform = buildContext.AvatarRootTransform;

            foreach (var root in avatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
            {
#if ATIV_DETECTED_VRM0
                if (avatarRootTransform.TryGetComponent<VRMMeta>(out var meta))
                {
                    meta.EnsureVRM0Components(avatarRootTransform, root, undoable);
                }

#endif
#if ATIV_DETECTED_VRM1
                if (avatarRootTransform.TryGetComponent<Vrm10Instance>(out var instance))
                {
                    instance.EnsureVRM1Components(avatarRootTransform, root, undoable);
                }
#endif
            }
        }

    }
}
