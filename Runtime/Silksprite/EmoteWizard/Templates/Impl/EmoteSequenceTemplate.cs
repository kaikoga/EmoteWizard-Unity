using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Impl
{
    public class EmoteSequenceTemplate : IEmoteTemplate
    {
        readonly EmoteTemplatePath _path;
        EmoteTemplatePath IEmoteTemplate.Path => _path;

        readonly EmoteSequenceFactory _sequence;

        public EmoteSequenceTemplate(EmoteTemplatePath path, EmoteSequenceFactory sequence)
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
