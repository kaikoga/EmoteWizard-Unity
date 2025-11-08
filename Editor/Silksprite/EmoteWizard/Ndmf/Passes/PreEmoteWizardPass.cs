using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Extensions;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.Components;
#endif

namespace Silksprite.EmoteWizard.Ndmf.Passes
{
    class PreEmoteWizardPass : Pass<PreEmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
            var avatarRootTransform = buildContext.AvatarRootTransform;

            foreach (var root in avatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
            {
#if EW_VRCSDK3_AVATARS
                if (buildContext.AvatarRootObject.TryGetComponent<VRCAvatarDescriptor>(out var avatarDescriptor))
                {
                    avatarDescriptor.DeleteVrcLayers(root);
                }
#endif
            }
        }
    }
}
