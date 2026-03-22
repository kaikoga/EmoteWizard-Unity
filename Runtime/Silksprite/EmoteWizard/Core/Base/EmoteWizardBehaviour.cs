using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

#if EW_VRCSDK3_AVATARS
using VRC.SDKBase;
#endif

#if !UNITY_2020_3_OR_NEWER
using System.Linq;
#endif

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBehaviour : MonoBehaviour
#if EW_VRCSDK3_AVATARS
        , IEditorOnly
#endif
    {
        public EmoteWizardEnvironment CreateEnv()
        {
            if (GetComponentInParent<EmoteWizardRoot>(true) is { } root)
            {
                return EmoteWizardEnvironment.FromRoot(root);
            }
            if (RuntimeUtil.FindAvatarInParents(transform) is { } avatarRoot)
            {
                return EmoteWizardEnvironment.FromAvatar(avatarRoot);
            }
            // We are dealing with avatar modules, it's okay env.Platform would be None
            var pseudoAvatarRoot = transform;
            while (pseudoAvatarRoot.parent)
            {
                pseudoAvatarRoot = pseudoAvatarRoot.parent;
            }
            return EmoteWizardEnvironment.FromAvatar(pseudoAvatarRoot);
        }
    }

#if !UNITY_2020_3_OR_NEWER
    internal static class ComponentExtensions
    {
        public static T GetComponentInParent<T>(this Component self, bool includeInactive)
            where T : Component
        {
            return self.GetComponentsInParent<T>(includeInactive).FirstOrDefault();
        }
    }
#endif

}