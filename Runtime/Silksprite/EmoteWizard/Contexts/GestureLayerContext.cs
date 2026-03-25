using JetBrains.Annotations;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.Utils;

namespace Silksprite.EmoteWizard.Contexts
{
    public class GestureLayerContext : AnimatorLayerContextBase
    {
        [UsedImplicitly]
        public GestureLayerContext(EmoteWizardEnvironment env) : base(env)
        {
            LayerOutputKind = LayerOutputKind.Gesture;
            DefaultAvatarMask = VrcSdkAssetLocator.HandsOnly();
        }

        public GestureLayerContext(EmoteWizardEnvironment env, AnimatorLayerConfigBase config) : base(env, config) { }
    }
}