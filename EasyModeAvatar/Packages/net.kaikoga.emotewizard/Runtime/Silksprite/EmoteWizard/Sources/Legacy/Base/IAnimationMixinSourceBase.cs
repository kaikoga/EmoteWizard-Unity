using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Legacy;

namespace Silksprite.EmoteWizard.Sources.Legacy.Base
{
    public interface IAnimationMixinSourceBase
    {
        IEnumerable<AnimationMixin> BaseMixins { get; }
        IEnumerable<AnimationMixin> Mixins { get; }
    }
}