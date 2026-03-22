using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Generic Emote Item Source", 1)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/generic_emote_item_source")]
    public class GenericEmoteItemSource : EmoteWizardDataSourceBase, IEmoteItemSource, IExpressionItemSource, IGenericEmoteItemSource, IEmoteTemplateSource
    {
        [SerializeField] public GenericEmoteTrigger trigger;
        [SerializeField] public EmoteSequenceSourceBase? sequence;

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate(EmoteWizardEnvironment environment)
        {
            return new GenericEmoteItemTemplate(EmoteTemplatePath.Context(CreateEnv(), this),
                trigger,
                sequence?.ToEmoteFactoryTemplate());
        }

        public EmoteSequenceSourceBase? FindEmoteSequenceSource()
        {
            if (sequence) return sequence;

            return GetComponents<EmoteSequenceSourceBase>() // Find in self
                .Concat(GetComponentsInParent<EmoteSequenceSourceBase>()) // then find in parents
                .Concat(GetComponentsInChildren<EmoteSequenceSourceBase>()) // then find in children
                .FirstOrDefault();
        }

        GenericEmoteItemTemplate ToTemplate()
        {
            IEmoteSequenceFactoryTemplate? FindEmoteFactory()
            {
                var source = FindEmoteSequenceSource();
                return source != null ? source.ToEmoteFactoryTemplate() : null;

            }

            // TODO cache me?
            return new GenericEmoteItemTemplate(EmoteTemplatePath.Context(CreateEnv(), this).Join(gameObject.name), trigger, FindEmoteFactory());
        }

        public IEnumerable<EmoteItem> ToEmoteItems(EmoteWizardEnvironment environment) => ToTemplate().ToEmoteItems(environment);
        public IEnumerable<GenericEmoteItem> ToGenericEmoteItems() => ToTemplate().ToGenericEmoteItems();

        public IEnumerable<ExpressionItem> ToExpressionItems(EmoteWizardEnvironment environment) => ToTemplate().ToExpressionItems();
    }
}