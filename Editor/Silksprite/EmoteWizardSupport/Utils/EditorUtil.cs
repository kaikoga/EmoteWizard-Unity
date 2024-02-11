using UnityEngine;

#if EW_NDMF_SUPPORT
using NdmfRuntimeUtil = nadena.dev.ndmf.runtime.RuntimeUtil;
#endif

namespace Silksprite.EmoteWizardSupport.Utils
{
    public static class EditorUtil
    {
        public static string RelativePath(GameObject root, GameObject child)
        {
#if EW_NDMF_SUPPORT
            return NdmfRuntimeUtil.RelativePath(root, child);
#endif

            return UnityEditor.AnimationUtility.CalculateTransformPath(child.transform, root.transform);
        }
    }
}