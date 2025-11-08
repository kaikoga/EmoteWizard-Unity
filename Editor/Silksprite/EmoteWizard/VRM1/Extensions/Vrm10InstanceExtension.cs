using System.Linq;
using Silksprite.AvatarTinkerVista.Ndmf;
using Silksprite.EmoteWizardSupport.Undoable;
using UniHumanoid;
using UnityEngine;
using UniVRM10;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class Vrm10InstanceExtension
    {
        public static void EnsureVRM1Components(this Vrm10Instance instance, Transform avatarRootTransform, EmoteWizardRoot root, EditorUndoable undoable)
        {
            if (!instance.Vrm)
            {
                instance.Vrm = ScriptableObject.CreateInstance<VRM10Object>();
                var missing = MissingMetaDefaults(avatarRootTransform);
                if (missing.nameOrTitle)
                {
                    instance.Vrm.Meta.Name = GuessOriginalAvatarName(avatarRootTransform);
                }

                if (missing.author && !string.IsNullOrWhiteSpace(root.author))
                {
                    instance.Vrm.Meta.Authors.Add(root.author);
                }

                if (missing.version)
                {
                    instance.Vrm.Meta.Version = "0.1.0";
                }
            }

            undoable.EnsureComponent<Humanoid>(avatarRootTransform, humanoid => humanoid.AssignBonesFromAnimator());
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