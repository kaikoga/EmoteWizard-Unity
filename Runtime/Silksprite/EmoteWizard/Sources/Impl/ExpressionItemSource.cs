using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Expression Item Source", 1000)]
    public class ExpressionItemSource : EmoteWizardDataSourceBase, IExpressionItemSource
    {
        [SerializeField] public ExpressionItem expressionItem;

        public IEnumerable<ExpressionItem> ToExpressionItems()
        {
            yield return expressionItem;
        }
    }
}