using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Sources.Base
{
    public interface IAnimationMixinSourceBase
    {
        IEnumerable<AnimationMixin> BaseMixins { get; }
        IEnumerable<AnimationMixin> Mixins { get; }
    }
}