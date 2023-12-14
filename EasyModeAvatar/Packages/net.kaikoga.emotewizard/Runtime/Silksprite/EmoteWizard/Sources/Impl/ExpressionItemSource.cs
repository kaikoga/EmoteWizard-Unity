using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class ExpressionItemSource : EmoteWizardDataSourceBase, IExpressionItemSource
    {
        [SerializeField] public ExpressionItem expressionItem;

        public IEnumerable<ExpressionItem> ToExpressionItems(ExpressionContext context)
        {
            yield return expressionItem;
        }
    }
}