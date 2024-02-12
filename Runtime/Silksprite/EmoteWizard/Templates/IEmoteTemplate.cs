using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates
{
    public interface IEmoteTemplate
    {
        string Path { get; }

        void PopulateSources(IUndoable undoable, Component target);
    }
}