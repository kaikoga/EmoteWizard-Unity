using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl.Multi
{
    public class MultiExpressionItemSource : EmoteWizardDataSourceBase, IExpressionItemSource
    {
        [SerializeField] public List<ExpressionItem> expressionItems = new List<ExpressionItem>(); 

        [SerializeField] public string defaultPrefix = "Default/";

        public IEnumerable<ExpressionItem> ExpressionItems => expressionItems;
    }
}