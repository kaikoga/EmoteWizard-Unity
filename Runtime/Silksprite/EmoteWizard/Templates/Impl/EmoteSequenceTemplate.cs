using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class EmoteSequenceTemplate : IEmoteTemplate
    {
        public EmoteTemplatePath Path { get; }

        readonly EmoteSequenceFactory _sequence;

        public EmoteSequenceTemplate(EmoteTemplatePath path, EmoteSequenceFactory sequence)
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
