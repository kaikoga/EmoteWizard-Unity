using UnityEngine;

#if EW_NDMF_SUPPORT
using NdmfRuntimeUtil = nadena.dev.ndmf.runtime.RuntimeUtil;
#endif

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.Components;
#endif

namespace Silksprite.EmoteWizardSupport.Utils
{
    public static class RuntimeUtil
    {
        public static Transform FindAvatarInParents(Transform transform)
        {
            if (!transform) return null;

#if EW_NDMF_SUPPORT
            return NdmfRuntimeUtil.FindAvatarInParents(transform);
#endif
            
#if EW_VRCSDK3_AVATARS
            return transform.GetComponentInParent<VRCAvatarDescriptor>()?.transform;
#endif

            return null;
        }
    }
}