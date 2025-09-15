using System.Linq;
using nadena.dev.ndmf;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

#if EW_ATIV_SUPPORT
using Silksprite.AvatarTinkerVista.Ndmf;
#endif

#if EW_VRM0
using VRM;
#endif

#if EW_VRM1
using UniHumanoid;
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
#if EW_VRM0
                if (avatarRootTransform.TryGetComponent<VRMMeta>(out var meta))
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

#endif
#if EW_VRM1
                if (avatarRootTransform.TryGetComponent<Vrm10Instance>(out var instance))
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
#endif
            }
        }

        string GuessOriginalAvatarName(Transform avatarRootTransform)
        {
            // probably ndmf manual build
            return avatarRootTransform.gameObject.name.Replace("(Clone)", "");
        }

        (bool nameOrTitle, bool author, bool version) MissingMetaDefaults(Transform avatarRootTransform)
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
