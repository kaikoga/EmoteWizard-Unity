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
    public class DefaultEmoteItemSource : EmoteWizardDataSourceBase, IEmoteItemSource, IExpressionItemSource, IGenericEmoteItemSource
    {
        [SerializeField] public EmoteItemKind emoteItemKind;
        [SerializeField] public EmoteSequenceFactoryKind emoteSequenceFactoryKind;
        
        [SerializeField] public LayerKind layerKind;
        [SerializeField] public HandSign handSign;

        IEmoteTemplate ToEmoteTemplate()
        {
            return DefaultEmoteItem.UnpackDefaultHandSign(emoteItemKind, emoteSequenceFactoryKind, CreateEnv().GetPlatformFeatures(), layerKind, handSign);
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

        public IEnumerable<GenericEmoteItem> ToGenericEmoteItems()
        {
            if (ToEmoteTemplate() is GenericEmoteItemTemplate genericEmoteTemplate)
            {
                return genericEmoteTemplate.ToGenericEmoteItems();
            }
            return Enumerable.Empty<GenericEmoteItem>();
        }
    }
}