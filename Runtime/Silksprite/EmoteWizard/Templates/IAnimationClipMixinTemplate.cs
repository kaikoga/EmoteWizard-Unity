using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Templates
{
    public interface IAnimationClipMixinTemplate : IEmoteTemplate
    {
        IEnumerable<AnimationClipMixin> ToAnimationClipMixins();
    }
}