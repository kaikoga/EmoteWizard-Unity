using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class GenericEmoteSequenceTemplate : IEmoteTemplate
    {
        public string Path { get; }
        readonly GenericEmoteSequenceFactory _sequence;

        public GenericEmoteSequenceTemplate(string path, GenericEmoteSequenceFactory sequence)
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
