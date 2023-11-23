using System;
using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Ndmf;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

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
            // generating.Run(PostEmoteWizardPass.Instance);
        }
    }

    class PreEmoteWizardPass : Pass<PreEmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
                
            foreach (var context in buildContext.AvatarRootTransform.GetComponentsInChildren<IEmoteWizardEnvironment>())
            {
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
        }
    }

    class EmoteWizardPass : Pass<EmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
            foreach (var env in buildContext.AvatarRootTransform.GetComponentsInChildren<IEmoteWizardEnvironment>())
            {
                var avatarContext = env.GetContext<IAvatarWizardContext>();
                if (avatarContext != null)
                {
                    avatarContext.AvatarDescriptor = buildContext.AvatarDescriptor;
                    var oldPersist = env.PersistGeneratedAssets;
                    env.PersistGeneratedAssets = false;
                    avatarContext.BuildAvatar();
                    env.PersistGeneratedAssets = oldPersist;
                    break;
                }
            }
        }
    }

    class PostEmoteWizardPass : Pass<PostEmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
            foreach (var ewComponent in buildContext.AvatarRootTransform.GetComponentsInChildren<EmoteWizardBehaviour>())
            {
                UnityEngine.Object.DestroyImmediate(ewComponent);
            }
        }
    }
}
