using System;
using System.Linq;
using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Ndmf;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

#if EW_ATIV_SUPPORT
using Silksprite.AvatarTinkerVista.Ndmf;
#endif

#if EW_VRCSDK3_AVATARS
using Silksprite.EmoteWizard.Contexts.Extensions;
using VRC.SDK3.Avatars.Components;
#endif

#if EW_VRM0
using Silksprite.EmoteWizard.Contexts.Extensions;
using VRM;
#endif

#if EW_VRM1
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.Extensions;
using UniHumanoid;
using UniVRM10;
#endif

[assembly: ExportsPlugin(typeof(EmoteWizardPlugin))]

namespace Silksprite.EmoteWizard.Ndmf
{
    class EmoteWizardPlugin : Plugin<EmoteWizardPlugin>
    {
        public override string QualifiedName => "net.kaikoga.emotewizard";
        public override string DisplayName => "Emote Wizard";

        protected override void OnUnhandledException(Exception e)
        {
            Debug.LogException(e);
        }

        protected override void Configure()
        {
            var resolving = InPhase(BuildPhase.Resolving);
            resolving.BeforePlugin("nadena.dev.modular-avatar").Run(PreEmoteWizardPass.Instance);
            var generating = InPhase(BuildPhase.Generating);
            generating.Run(EmoteWizardPass.Instance);
            generating.Run(PostEmoteWizardPass.Instance);
        }
    }

    class PreEmoteWizardPass : Pass<PreEmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
            var undoable = new EditorUndoable("Prepare Emote Wizard from ndmf");

            var avatarRootTransform = buildContext.AvatarRootTransform;
            (bool nameOrTitle, bool author, bool version) MissingMetaDefaults()
            {
#if EW_ATIV_SUPPORT
                var overwrites = avatarRootTransform.GetComponentsInChildren<AtivOverwriteVrmMeta>();
                return (overwrites.All(c => !c.nameOrTitle.willOverwrite),
                    overwrites.All(c => !c.author.willOverwrite),
                    overwrites.All(c => !c.version.willOverwrite));
#else
                return (true, true, true);
#endif
            }

            string GuessOriginalAvatarName()
            {
                // probably ndmf manual build
                return avatarRootTransform.gameObject.name.Replace("(Clone)", "");
            }

            foreach (var root in avatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
            {
#if EW_VRCSDK3_AVATARS
                if (buildContext.AvatarDescriptor is VRCAvatarDescriptor avatarDescriptor)
                {
                    DeleteVrcLayers(root, avatarDescriptor);
                }
#endif
#if EW_VRM0
                if (avatarRootTransform.GetComponent<VRMMeta>() is VRMMeta meta)
                {
                    if (!meta.Meta)
                    {
                        meta.Meta = ScriptableObject.CreateInstance<VRMMetaObject>();
                        var missing = MissingMetaDefaults();
                        if (missing.nameOrTitle)
                        {
                            meta.Meta.Title = GuessOriginalAvatarName();
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
                if (avatarRootTransform.GetComponent<Vrm10Instance>() is Vrm10Instance instance)
                {
                    if (!instance.Vrm)
                    {
                        instance.Vrm = ScriptableObject.CreateInstance<VRM10Object>();
                        var missing = MissingMetaDefaults();
                        if (missing.nameOrTitle)
                        {
                            instance.Vrm.Meta.Name = GuessOriginalAvatarName();
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

#if EW_VRCSDK3_AVATARS
        static void DeleteVrcLayers(EmoteWizardRoot root, VRCAvatarDescriptor avatarDescriptor)
        {
            root.ToEnv().DisconnectAllOutputAssets();

            DeleteLayer(avatarDescriptor.baseAnimationLayers, VRCAvatarDescriptor.AnimLayerType.FX);
            DeleteLayer(avatarDescriptor.baseAnimationLayers, VRCAvatarDescriptor.AnimLayerType.Gesture);
            DeleteLayer(avatarDescriptor.baseAnimationLayers, VRCAvatarDescriptor.AnimLayerType.Action);
            DeleteLayer(avatarDescriptor.specialAnimationLayers, VRCAvatarDescriptor.AnimLayerType.Sitting);

            void DeleteLayer(VRCAvatarDescriptor.CustomAnimLayer[] layers, VRCAvatarDescriptor.AnimLayerType animLayerType)
            {
                for (var i = 0; i < layers.Length; i++)
                {
                    var layer = layers[i];
                    {
                        if (layer.type == animLayerType)
                        {
                            layer.animatorController = null;
                            layer.mask = null;
                        }
                    }
                    layers[i] = layer;
                }
            }
        }
#endif
    }

    class EmoteWizardPass : Pass<EmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
            foreach (var root in buildContext.AvatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
            {
                var env = root.ToEnv();
                env.PersistGeneratedAssets = false;
                env.AvatarRoot = buildContext.AvatarRootTransform;
#if EW_VRCSDK3_AVATARS
                env.BuildVrcAvatar(new EditorUndoable("Build Emote Wizard from ndmf"), false);
#endif
#if EW_VRM0
                env.BuildVrm0Avatar(new EditorUndoable("Build Emote Wizard from ndmf"), false);
#endif
#if EW_VRM1
                env.BuildVrm1Avatar(new EditorUndoable("Build Emote Wizard from ndmf"), false);
#endif
            }
        }
    }

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
