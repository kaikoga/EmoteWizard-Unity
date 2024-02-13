using System;
using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Ndmf;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

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

            foreach (var root in buildContext.AvatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
            {
#if EW_VRCSDK3_AVATARS
                if (buildContext.AvatarDescriptor is VRCAvatarDescriptor avatarDescriptor)
                {
                    DeleteVrcLayers(root, avatarDescriptor);
                }
#endif
#if EW_VRM0
                if (buildContext.AvatarRootTransform.GetComponent<VRMMeta>() is VRMMeta meta)
                {
                    if (!meta.Meta) meta.Meta = ScriptableObject.CreateInstance<VRMMetaObject>();
                }

                var blendShapeProxy = undoable.EnsureComponent<VRMBlendShapeProxy>(buildContext.AvatarRootTransform);
                if (!blendShapeProxy.BlendShapeAvatar) blendShapeProxy.BlendShapeAvatar = ScriptableObject.CreateInstance<BlendShapeAvatar>();

                undoable.EnsureComponent<VRMFirstPerson>(buildContext.AvatarRootTransform);
#endif
#if EW_VRM1
                if (buildContext.AvatarRootTransform.GetComponent<Vrm10Instance>() is Vrm10Instance instance)
                {
                    if (!instance.Vrm) instance.Vrm = ScriptableObject.CreateInstance<VRM10Object>();
                }

                undoable.EnsureComponent<Humanoid>(buildContext.AvatarRootTransform, humanoid => humanoid.AssignBonesFromAnimator());
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
