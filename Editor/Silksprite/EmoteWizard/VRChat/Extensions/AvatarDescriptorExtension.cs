using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class AvatarDescriptorExtension
    {
        public static IEnumerable<AnimatorController> AllAnimationLayers(this VRCAvatarDescriptor avatarDescriptor)
        {
            return AllCustomAnimLayers(avatarDescriptor)
                .Select(layer => layer.animatorController as AnimatorController)
                .Where(layer => layer != null)
                .Distinct();
        }

        public static AnimatorController FindAnimationLayer(this VRCAvatarDescriptor avatarDescriptor, VRCAvatarDescriptor.AnimLayerType vrcLayerType)
        {
            return FindCustomAnimLayer(avatarDescriptor, vrcLayerType).animatorController as AnimatorController;
        }

        public static VRCAvatarDescriptor.CustomAnimLayer FindCustomAnimLayer(this VRCAvatarDescriptor avatarDescriptor, VRCAvatarDescriptor.AnimLayerType vrcLayerType)
        {
            return AllCustomAnimLayers(avatarDescriptor)
                .FirstOrDefault(layer => layer.type == vrcLayerType);
        }

        static IEnumerable<VRCAvatarDescriptor.CustomAnimLayer> AllCustomAnimLayers(this VRCAvatarDescriptor avatarDescriptor)
        {
            if (avatarDescriptor.baseAnimationLayers != null)
            {
                foreach (var layer in avatarDescriptor.baseAnimationLayers)
                {
                    yield return layer;
                }
            }
            if (avatarDescriptor.specialAnimationLayers != null)
            {
                foreach (var layer in avatarDescriptor.specialAnimationLayers)
                {
                    yield return layer;
                }
            }
        }

        public static void DeleteVrcLayers(this VRCAvatarDescriptor avatarDescriptor, EmoteWizardRoot root)
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
    }
}