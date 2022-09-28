using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Internal;

namespace Silksprite.EmoteWizard.Sources.Extensions
{
    public static class ExpressionItemSourceExtension
    {
        public static void RepopulateDefaultExpressionItems(this ExpressionItemSource expressionItemSource)
        {
            expressionItemSource.expressionItems = new List<ExpressionItem>();
            PopulateDefaultExpressionItems(expressionItemSource);
        }

        public static void PopulateDefaultExpressionItems(this ExpressionItemSource expressionItemSource)
        {
            if (expressionItemSource.expressionItems == null) expressionItemSource.expressionItems = new List<ExpressionItem>();

            expressionItemSource.expressionItems = DefaultActionEmote.PopulateDefaultExpressionItems(expressionItemSource.defaultPrefix, expressionItemSource.expressionItems);
        }
    }
}