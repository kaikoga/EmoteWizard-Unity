using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Expression Item Source", 1000)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/expression_item_source")]
    public class ExpressionItemSource : EmoteWizardDataSourceBase, IExpressionItemSource, IEmoteTemplateSource
    {
        [SerializeField] public ExpressionItem expressionItem = new ExpressionItem();

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate(EmoteWizardEnvironment environment)
        {
            return new ExpressionItemTemplate(EmoteTemplatePath.Context(CreateEnv(), this),
                expressionItem);
        }


        public IEnumerable<ExpressionItem> ToExpressionItems(EmoteWizardEnvironment environment)
        {
            if (expressionItem.IsValid) yield return expressionItem;
        }
    }
}