using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class AnimationClipMixinTemplate : IAnimationClipMixinTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        readonly AnimationClipMixin _mixin;

        public AnimationClipMixinTemplate(EmoteTemplatePath path, AnimationClipMixin mixin)
        {
            _path = path;
            _mixin = mixin;
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<AnimationClipMixinSource>(target);
            source.mixin = _mixin;
        }

        IEnumerable<AnimationClipMixin> IAnimationClipMixinTemplate.ToAnimationClipMixins()
        {
            yield return _mixin;
        }
    }
}
