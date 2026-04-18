using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Templates
{
    public interface IAnimatorControllerMixinTemplate : IEmoteTemplate
    {
        IEnumerable<AnimatorControllerMixin> ToAnimatorControllerMixins();
    }
}