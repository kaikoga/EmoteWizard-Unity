using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class GenericEmoteSequenceTemplate : IEmoteTemplate
    {
        public EmoteTemplatePath Path { get; }

        readonly GenericEmoteSequenceFactory _sequence;

        public GenericEmoteSequenceTemplate(EmoteTemplatePath path, GenericEmoteSequenceFactory sequence)
        {
            Path = path;
            _sequence = sequence;
        }

        public void PopulateSources(IUndoable undoable, Component target)
        {
            _sequence.PopulateSequenceSource(undoable, target);
        }
    }
}
