using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class AnimatorControllerMixinTemplate : IAnimatorControllerMixinTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        readonly AnimatorControllerMixin _mixin;

        public AnimatorControllerMixinTemplate(EmoteTemplatePath path, AnimatorControllerMixin mixin)
        {
            _path = path;
            _mixin = mixin;
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<AnimatorControllerMixinSource>(target);
            source.mixin = _mixin;
        }

        IEnumerable<AnimatorControllerMixin> IAnimatorControllerMixinTemplate.ToAnimatorControllerMixins()
        {
            yield return _mixin;
        }
    }
}
