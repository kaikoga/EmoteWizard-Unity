using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class ExpressionItemSource : EmoteWizardDataSourceBase, IExpressionItemSource
    {
        [SerializeField] public ExpressionItem expressionItem; 

        public IEnumerable<ExpressionItem> ExpressionItems
        {
            get { yield return expressionItem; }
        }
    }
}