using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Wizards;
using Silksprite.EmoteWizard.Wizards.Defaults;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Default Emote Item Source", 910)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/default_emote_item_source")]
    public class DefaultEmoteItemSource : EmoteWizardBase, IEmoteItemSource, IExpressionItemSource, IGenericEmoteItemSource
    {
        [SerializeField] public EmoteItemKind emoteItemKind;
        [SerializeField] public EmoteSequenceFactoryKind emoteSequenceFactoryKind;
        
        [SerializeField] public LayerKind layerKind;
        [SerializeField] public HandSign handSign;

        protected override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            yield return ToEmoteTemplate();
        }

        IEmoteTemplate ToEmoteTemplate()
        {
            return DefaultEmoteItem.UnpackDefaultHandSign(emoteItemKind, emoteSequenceFactoryKind, CreateEnv().GetPlatformFeatures(), layerKind, handSign);
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

        public IEnumerable<GenericEmoteItem> ToGenericEmoteItems()
        {
            var emoteTemplate = ToEmoteTemplate();
            if (emoteTemplate is GenericEmoteItemTemplate genericEmoteTemplate)
            {
                return genericEmoteTemplate.ToGenericEmoteItems();
            }
            return Enumerable.Empty<GenericEmoteItem>();
        }
    }
}