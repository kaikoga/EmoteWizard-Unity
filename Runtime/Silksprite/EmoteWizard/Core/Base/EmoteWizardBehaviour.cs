using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

#if EW_VRCSDK3_AVATARS
using VRC.SDKBase;
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
            if (this.GetComponentInParent<EmoteWizardRoot>(true) is EmoteWizardRoot root)
            {
                return EmoteWizardEnvironment.FromRoot(root);
            }
            if (RuntimeUtil.FindAvatarInParents(transform) is Transform avatarRoot)
            {
                return EmoteWizardEnvironment.FromAvatar(avatarRoot);
            }
            return null;
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