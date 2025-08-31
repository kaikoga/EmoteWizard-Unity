using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Ndmf.Passes
{
    class PostEmoteWizardPass : Pass<PostEmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
            foreach (var ewComponent in buildContext.AvatarRootTransform.GetComponentsInChildren<EmoteWizardBehaviour>(true))
            {
                UnityEngine.Object.DestroyImmediate(ewComponent);
            }
        }
    }
}
