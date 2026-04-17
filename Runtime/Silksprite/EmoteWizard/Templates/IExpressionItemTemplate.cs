using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Templates
{
    public interface IExpressionItemTemplate : IEmoteTemplate
    {
        IEnumerable<ExpressionItem> ToExpressionItems();
    }
}