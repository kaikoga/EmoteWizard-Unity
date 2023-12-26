using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Sources.Templates
{
    public interface IEmoteTemplate
    {
        IEnumerable<EmoteItem> ToEmoteItems();
        IEnumerable<ExpressionItem> ToExpressionItems();
    }
}