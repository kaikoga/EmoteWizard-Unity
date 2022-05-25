using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class GestureWizardExtension
    {
        public static RuntimeAnimatorController BuildOutputAsset(this GestureWizard gestureWizard, ParametersWizard parametersWizard)
        {
            var builder = new AnimationControllerBuilder
            {
                AnimationWizardBase = gestureWizard,
                ParametersWizard = parametersWizard,
                DefaultRelativePath = "Gesture/@@@Generated@@@Gesture.controller"
            };

            var defaultAvatarMask = gestureWizard.defaultAvatarMask ? gestureWizard.defaultAvatarMask : VrcSdkAssetLocator.HandsOnly();

            builder.BuildStaticLayer("Reset", null, defaultAvatarMask);
            builder.BuildMixinLayers(gestureWizard.CollectBaseMixins());
            builder.BuildHandSignLayer("Left Hand", true);
            builder.BuildHandSignLayer("Right Hand", false);
            builder.BuildParameterLayers(gestureWizard.CollectParameterEmotes());
            builder.BuildMixinLayers(gestureWizard.CollectMixins());

            builder.BuildTrackingControlLayers();
            builder.BuildParameters();
            
            return gestureWizard.outputAsset;
        }
    }
}