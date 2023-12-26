using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Templates;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBase : EmoteWizardDataSourceBase, IExpressionItemSource, IEmoteItemSource
    {
        protected abstract IEnumerable<EmoteItemTemplate> SourceTemplates();
        
        public virtual IEnumerable<ExpressionItem> ToExpressionItems()
        {
            foreach (var template in SourceTemplates())
            {
                foreach (var expressionItem in template.ToExpressionItems()) yield return expressionItem;
            }
        }

        public virtual IEnumerable<EmoteItem> ToEmoteItems()
        {
            foreach (var template in SourceTemplates())
            {
                foreach (var emoteItem in template.ToEmoteItems()) yield return emoteItem;
            }
        }

    }
}