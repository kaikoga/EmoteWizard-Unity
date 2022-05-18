using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    public class ExpressionItemSource : EmoteWizardDataSourceBase
    {
        [SerializeField] public List<ExpressionItem> expressionItems; 

        [SerializeField] public string defaultPrefix = "Default/";
    }
}