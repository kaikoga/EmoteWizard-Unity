using Silksprite.EmoteWizard.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class FxWizardExtension
    {
        public static RuntimeAnimatorController BuildOutputAsset(this FxWizard fxWizard, ParametersWizard parametersWizard)
        {
            var builder = new AnimationControllerBuilder
            {
                AnimationWizardBase = fxWizard,
                ParametersWizard = parametersWizard,
                DefaultRelativePath = "FX/@@@Generated@@@FX.controller"
            };

            var resetClip = fxWizard.EmoteWizardRoot.EnsureAsset("FX/@@@Generated@@@ResetFX.anim", ref fxWizard.resetClip);
            fxWizard.BuildResetClip(resetClip);

            builder.BuildStaticLayer("Reset", resetClip, null);
            builder.BuildMixinLayers(fxWizard.CollectBaseMixins());
            builder.BuildHandSignLayer("Left Hand", true);
            builder.BuildHandSignLayer("Right Hand", false);
            builder.BuildParameterLayers(fxWizard.CollectParameterEmotes());
            builder.BuildMixinLayers(fxWizard.CollectMixins());

            builder.BuildTrackingControlLayers();
            builder.BuildParameters();
            
            return fxWizard.outputAsset;
        }
    }
}