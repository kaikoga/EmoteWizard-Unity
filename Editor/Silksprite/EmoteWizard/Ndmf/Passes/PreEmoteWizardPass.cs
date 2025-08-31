using nadena.dev.ndmf;
using Silksprite.EmoteWizard.Contexts;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.Components;
#endif

namespace Silksprite.EmoteWizard.Ndmf.Passes
{
    class PreEmoteWizardPass : Pass<PreEmoteWizardPass>
    {
        protected override void Execute(BuildContext buildContext)
        {
            var avatarRootTransform = buildContext.AvatarRootTransform;

            foreach (var root in avatarRootTransform.GetComponentsInChildren<EmoteWizardRoot>(true))
            {
#if EW_VRCSDK3_AVATARS
                if (buildContext.AvatarRootObject.TryGetComponent<VRCAvatarDescriptor>(out var avatarDescriptor))
                {
                    DeleteVrcLayers(root, avatarDescriptor);
                }
#endif
            }
        }

#if EW_VRCSDK3_AVATARS
        static void DeleteVrcLayers(EmoteWizardRoot root, VRCAvatarDescriptor avatarDescriptor)
        {
            var env = root.ToEnv(); 
            env.DisconnectAllOutputAssets();

            DeleteLayer(avatarDescriptor.baseAnimationLayers, VRCAvatarDescriptor.AnimLayerType.FX);
            if (env.OverrideGesture != OverrideGeneratedControllerType2.Inherit)
            {
                DeleteLayer(avatarDescriptor.baseAnimationLayers, VRCAvatarDescriptor.AnimLayerType.Gesture);
            }
            if (env.OverrideAction != OverrideGeneratedControllerType1.Inherit)
            {
                DeleteLayer(avatarDescriptor.baseAnimationLayers, VRCAvatarDescriptor.AnimLayerType.Action);
            }
            if (env.OverrideSitting != OverrideControllerType2.Inherit)
            {
                DeleteLayer(avatarDescriptor.specialAnimationLayers, VRCAvatarDescriptor.AnimLayerType.Sitting);
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
#endif
    }
}
