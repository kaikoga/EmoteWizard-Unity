using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Sources.Impl.Multi;

namespace Silksprite.EmoteWizard.Sources.Multi.Extensions
{
    public static class MultiExpressionItemSourceExtension
    {
        public static void RepopulateDefaultExpressionItems(this MultiExpressionItemSource multiExpressionItemSource)
        {
            multiExpressionItemSource.expressionItems = new List<ExpressionItem>();
            PopulateDefaultExpressionItems(multiExpressionItemSource);
        }

        public static void PopulateDefaultExpressionItems(this MultiExpressionItemSource multiExpressionItemSource)
        {
            if (multiExpressionItemSource.expressionItems == null) multiExpressionItemSource.expressionItems = new List<ExpressionItem>();

            multiExpressionItemSource.expressionItems = DefaultActionEmote.PopulateDefaultExpressionItems(multiExpressionItemSource.defaultPrefix, multiExpressionItemSource.expressionItems);
        }
    }
}