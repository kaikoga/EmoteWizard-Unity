using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Wizards.Defaults;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Default Action Emote Item Source", 911)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/default_action_emote_item_source")]
    public class DefaultActionEmoteItemSource : EmoteWizardDataSourceBase, IEmoteItemSource, IExpressionItemSource
    {
        [SerializeField] public DefaultActionIndex defaultActionIndex;

        IEmoteTemplate ToEmoteTemplate()
        {
            return DefaultActionEmote.UnpackDefaultAction(defaultActionIndex);
        }

        // TODO: expose EmoteTemplate -> EmoteItem
        public IEnumerable<EmoteItem> ToEmoteItems(EmoteWizardEnvironment environment)
        {
            if (ToEmoteTemplate() is EmoteItemTemplate emoteTemplate)
            {
                return emoteTemplate.ToEmoteItems(environment);
            }
            if (ToEmoteTemplate() is GenericEmoteItemTemplate genericEmoteTemplate)
            {
                return genericEmoteTemplate.ToEmoteItems(environment);
            }
            return Enumerable.Empty<EmoteItem>();
        }

        public IEnumerable<ExpressionItem> ToExpressionItems(EmoteWizardEnvironment environment)
        {
            if (ToEmoteTemplate() is EmoteItemTemplate emoteTemplate)
            {
                return emoteTemplate.ToExpressionItems(environment);
            }
            if (ToEmoteTemplate() is GenericEmoteItemTemplate genericEmoteTemplate)
            {
                return genericEmoteTemplate.ToExpressionItems();
            }
            return Enumerable.Empty<ExpressionItem>();
        }
    }
}