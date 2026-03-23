using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Generic Emote Item Source", 1)]
    [HelpURL("https://docs.kaikoga.net/emotewizard/sources/generic_emote_item_source")]
    public class GenericEmoteItemSource : EmoteWizardDataSourceBase, IEmoteTemplateSource
    {
        [SerializeField] public GenericEmoteTrigger trigger;
        [SerializeField] public EmoteSequenceSourceBase? sequence;

        IEmoteTemplate IEmoteTemplateSource.ToEmoteTemplate()
        {
            return new GenericEmoteItemTemplate(SelfPath,
                trigger,
                FindEmoteSequenceSource()?.ToEmoteFactoryTemplate() // TODO: sequence?.ToEmoteFactoryTemplate()) after sequence resolving in templates 
            ); 
        }

        public EmoteSequenceSourceBase? FindEmoteSequenceSource()
        {
            if (sequence) return sequence;

            return GetComponents<EmoteSequenceSourceBase>() // Find in self
                .Concat(GetComponentsInParent<EmoteSequenceSourceBase>()) // then find in parents
                .Concat(GetComponentsInChildren<EmoteSequenceSourceBase>()) // then find in children
                .FirstOrDefault();
        }
    }
}