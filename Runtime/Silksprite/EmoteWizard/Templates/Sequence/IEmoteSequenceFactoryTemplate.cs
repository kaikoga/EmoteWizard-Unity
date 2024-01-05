using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates.Sequence
{
    public interface IEmoteSequenceFactoryTemplate : IEmoteSequenceFactory
    {
        EmoteSequenceSourceBase PopulateSequenceSource(IUndoable undoable, Component target);
    }
}