using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Templates
{
    public interface IEmoteTemplate
    {
        IEnumerable<EmoteItem> ToEmoteItems();
        IEnumerable<ExpressionItem> ToExpressionItems();

        void PopulateSources(Component target);
    }
}