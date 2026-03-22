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
    public class DefaultActionEmoteItemSource : EmoteWizardBase, IEmoteItemSource, IExpressionItemSource
    {
        [SerializeField] public DefaultActionIndex defaultActionIndex;

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            yield return ToEmoteTemplate();
        }

        IEmoteTemplate ToEmoteTemplate()
        {
            return DefaultActionEmote.UnpackDefaultAction(EmoteTemplatePath.Context(CreateEnv(), this), defaultActionIndex);
        }

        // TODO: expose EmoteTemplate -> EmoteItem
        public IEnumerable<EmoteItem> ToEmoteItems(EmoteWizardEnvironment environment)
        {
            return ToEmoteTemplate() switch
            {
                EmoteItemTemplate emoteTemplate => emoteTemplate.ToEmoteItems(environment),
                GenericEmoteItemTemplate genericEmoteTemplate => genericEmoteTemplate.ToEmoteItems(environment),
                _ => Enumerable.Empty<EmoteItem>()
            };
        }

        public IEnumerable<ExpressionItem> ToExpressionItems(EmoteWizardEnvironment environment)
        {
            return ToEmoteTemplate() switch
            {
                EmoteItemTemplate emoteTemplate => emoteTemplate.ToExpressionItems(environment),
                GenericEmoteItemTemplate genericEmoteTemplate => genericEmoteTemplate.ToExpressionItems(),
                _ => Enumerable.Empty<ExpressionItem>()
            };
        }
    }
}