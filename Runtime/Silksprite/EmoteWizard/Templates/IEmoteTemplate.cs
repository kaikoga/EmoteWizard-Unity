using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates
{
    public interface IEmoteTemplate
    {
        EmoteTemplatePath Path { get; }

        void PopulateSources(IUndoable undoable, Component target);
    }
}