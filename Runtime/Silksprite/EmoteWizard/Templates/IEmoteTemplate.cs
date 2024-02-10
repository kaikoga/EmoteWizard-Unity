using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates
{
    public interface IEmoteTemplate
    {
        string Path { get; }

        IEnumerable<EmoteItem> ToEmoteItems();
        IEnumerable<ExpressionItem> ToExpressionItems();

        void PopulateSources(IUndoable undoable, Component target);
    }
}