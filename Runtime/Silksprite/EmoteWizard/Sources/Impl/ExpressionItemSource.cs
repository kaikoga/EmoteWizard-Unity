using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Expression Item Source", 1000)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/expression_item_source")]
    public class ExpressionItemSource : EmoteWizardDataSourceBase, IEmoteTemplateSource
    {
        [SerializeField] public ExpressionItem expressionItem = new ExpressionItem();

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate()
        {
            return new ExpressionItemTemplate(SelfPath, expressionItem);
        }
    }
}