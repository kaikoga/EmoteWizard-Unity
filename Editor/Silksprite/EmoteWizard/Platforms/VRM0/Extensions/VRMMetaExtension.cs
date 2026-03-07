using System.Linq;
using Silksprite.AvatarTinkerVista;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;
using VRM;

namespace Silksprite.EmoteWizard.Platforms.VRM0.Extensions
{
    public static class VRMMetaExtension
    {
        public static void EnsureVRM0Components(this VRMMeta meta, Transform avatarRootTransform, EmoteWizardRoot root, EditorUndoable undoable)
        {

            if (!meta.Meta)
            {
                meta.Meta = ScriptableObject.CreateInstance<VRMMetaObject>();
                var missing = MissingMetaDefaults(avatarRootTransform);
                if (missing.nameOrTitle)
                {
                    meta.Meta.Title = GuessOriginalAvatarName(avatarRootTransform);
                }

                if (missing.author && !string.IsNullOrWhiteSpace(root.author))
                {
                    meta.Meta.Author = root.author;
                }

                if (missing.version)
                {
                    meta.Meta.Version = "0.1.0";
                }
            }

            var blendShapeProxy = undoable.EnsureComponent<VRMBlendShapeProxy>(avatarRootTransform);
            if (!blendShapeProxy.BlendShapeAvatar)
            {
                blendShapeProxy.BlendShapeAvatar = ScriptableObject.CreateInstance<BlendShapeAvatar>();
            }

            undoable.EnsureComponent<VRMFirstPerson>(avatarRootTransform);
        }

        static string GuessOriginalAvatarName(Transform avatarRootTransform)
        {
            // probably ndmf manual build
            return avatarRootTransform.gameObject.name.Replace("(Clone)", "");
        }

        static (bool nameOrTitle, bool author, bool version) MissingMetaDefaults(Transform avatarRootTransform)
        {
#if EW_ATIV_SUPPORT
            var overwrites = avatarRootTransform.GetComponentsInChildren<AtivOverwriteVRMMeta>();
            return (overwrites.All(c => !c.nameOrTitle.willOverwrite),
                overwrites.All(c => !c.author.willOverwrite),
                overwrites.All(c => !c.version.willOverwrite));
#else
                return (true, true, true);
#endif
        }
    }
}