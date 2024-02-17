using System.IO;
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
        const string AvatarRootMagic = "$$$AVATAR_ROOT$$$"; // this follows the convention of Modular Avatar

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

        public static string RelativePath(Transform root, Transform child)
        {
            return RelativePath(root, child, AvatarRootMagic);
        }

        public static string CalculateAnimationTransformPath(Transform root, Transform child)
        {
            return RelativePath(root, child, "");
        }

        public static Transform FromRelativePath(Transform root, string relativePath)
        {
            return FromRelativePath(root, relativePath, AvatarRootMagic);
        }

        static string RelativePath(Transform root, Transform child, string rootName)
        {
            if (root == child) return rootName;
            if (!child) return null;

            var cursor = child;
            var path = child.gameObject.name;
            while (true)
            {
                cursor = cursor.parent;
                if (cursor == root) break;
                if (!cursor) break;
                path = Path.Combine(cursor.gameObject.name, path);
            }
            return path;
        }

        static Transform FromRelativePath(Transform root, string relativePath, string rootName)
        {
            return relativePath == rootName ? root : root.Find(relativePath);
        }
    }
}