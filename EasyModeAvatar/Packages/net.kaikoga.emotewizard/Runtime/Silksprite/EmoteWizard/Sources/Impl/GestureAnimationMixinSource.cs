using Silksprite.EmoteWizard.Sources.Base;
using Silksprite.EmoteWizard.Sources.Impl.Base;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class GestureAnimationMixinSource : AnimationMixinSourceBase, IGestureAnimationMixinSource
    {
        public override string LayerName => "Gesture";
    }
}