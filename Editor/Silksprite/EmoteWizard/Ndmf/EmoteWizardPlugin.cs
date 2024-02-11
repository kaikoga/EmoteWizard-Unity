using System;
using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Ndmf;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.Components;
using Silksprite.EmoteWizard.Contexts.Extensions;
#endif

#if EW_VRM0
using Silksprite.EmoteWizard.Contexts.Extensions;
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
#if EW_VRCSDK3_AVATARS
            foreach (var root in buildContext.AvatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
            {
                var context = root.ToEnv();
                context.DisconnectAllOutputAssets();

                DeleteLayer(buildContext.AvatarDescriptor.baseAnimationLayers, VRCAvatarDescriptor.AnimLayerType.FX);
                DeleteLayer(buildContext.AvatarDescriptor.baseAnimationLayers, VRCAvatarDescriptor.AnimLayerType.Gesture);
                DeleteLayer(buildContext.AvatarDescriptor.baseAnimationLayers, VRCAvatarDescriptor.AnimLayerType.Action);
                DeleteLayer(buildContext.AvatarDescriptor.specialAnimationLayers, VRCAvatarDescriptor.AnimLayerType.Sitting);

                break;
            }
            
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
#endif
        }
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
