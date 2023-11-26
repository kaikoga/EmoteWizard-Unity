using System.Collections.Generic;
using System.Linq;
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
            return AllCustomAnimLayers(avatarDescriptor)
                .Where(layer => layer.type == vrcLayerType)
                .Select(layer => layer.animatorController as AnimatorController)
                .FirstOrDefault();
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
    }
}