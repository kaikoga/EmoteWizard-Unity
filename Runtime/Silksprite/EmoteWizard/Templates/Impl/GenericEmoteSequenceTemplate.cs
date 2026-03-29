using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class GenericEmoteSequenceTemplate : IEmoteTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        readonly GenericEmoteSequenceFactory _sequence;

        public GenericEmoteSequenceTemplate(EmoteTemplatePath path, GenericEmoteSequenceFactory sequence)
        {
            _path = path;
            _sequence = sequence;
        }

        void IEmoteTemplate.PopulateSources(IUndoable undoable, Component target)
        {
            _sequence.PopulateSequenceSource(undoable, target);
        }
    }
}
