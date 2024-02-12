using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class GenericEmoteTriggerTemplate : IEmoteTemplate
    {
        public string Path { get; }
        readonly GenericEmoteTrigger _trigger;

        public GenericEmoteTriggerTemplate(string path, GenericEmoteTrigger trigger)
        {
            Path = path;
            _trigger = trigger;
        }

        public void PopulateSources(IUndoable undoable, Component target)
        {
            var source = undoable.AddComponent<GenericEmoteItemSource>(target);
            source.trigger = _trigger;
        }
    }
}
