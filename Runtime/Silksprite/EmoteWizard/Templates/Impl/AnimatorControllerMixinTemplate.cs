using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class AnimatorControllerMixinTemplate : IAnimatorControllerMixinTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        public AnimatorControllerMixinTemplate(EmoteTemplatePath path)
        {
            _path = path;
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<AnimatorControllerMixinSource>(target);
        }
    }
}
