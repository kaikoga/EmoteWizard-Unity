using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Utils;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class GestureWizardExtension
    {
        public static void BuildOutputAsset(this GestureWizard gestureWizard, ParametersWizard parametersWizard)
        {
            var builder = new AnimationControllerBuilder
            {
                AnimationWizardBase = gestureWizard,
                ParametersWizard = parametersWizard,
                DefaultRelativePath = "Gesture/@@@Generated@@@Gesture.controller"
            };

            var defaultAvatarMask = gestureWizard.defaultAvatarMask ? gestureWizard.defaultAvatarMask : VrcSdkAssetLocator.HandsOnly();

            builder.BuildStaticLayer("Reset", null, defaultAvatarMask);
            builder.BuildMixinLayers(gestureWizard.baseMixins);
            builder.BuildHandSignLayer("Left Hand", true, gestureWizard.advancedAnimations);
            builder.BuildHandSignLayer("Right Hand", false, gestureWizard.advancedAnimations);
            builder.BuildParameterLayers(gestureWizard.ActiveParameters);
            builder.BuildMixinLayers(gestureWizard.mixins);

            builder.BuildTrackingControlLayers();
            builder.BuildParameters();
        }
    }
}