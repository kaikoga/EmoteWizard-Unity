using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Utils;

namespace Silksprite.EmoteWizard.Contexts
{
    public class GestureLayerContext : AnimatorLayerContextBase
    {
        [UsedImplicitly]
        public GestureLayerContext(EmoteWizardEnvironment env) : base(env)
        {
            LayerKind = LayerKind.Gesture;
            DefaultAvatarMask = VrcSdkAssetLocator.HandsOnly();
        }

        public GestureLayerContext(EmoteWizardEnvironment env, AnimatorLayerWizardBase wizard) : base(env, wizard) { }
    }
}