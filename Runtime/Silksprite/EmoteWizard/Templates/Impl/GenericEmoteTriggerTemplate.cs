using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class GenericEmoteTriggerTemplate : IEmoteTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        readonly GenericEmoteTrigger _trigger;

        public GenericEmoteTriggerTemplate(EmoteTemplatePath path, GenericEmoteTrigger trigger)
        {
            _path = path;
            _trigger = trigger;
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<GenericEmoteItemSource>(target);
            source.trigger = _trigger;
        }
    }
}
