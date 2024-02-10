using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Templates.Sequence;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    [AddComponentMenu("Emote Wizard/Sources/Emote Item Source", 0)]
    public class GenericEmoteItemSource : EmoteWizardDataSourceBase, IEmoteItemSource, IExpressionItemSource
    {
        [SerializeField] public HandSign handSign;

        [SerializeField] public EmoteSequenceSourceBase sequence;

        public EmoteSequenceSourceBase FindEmoteSequenceSource()
        {
            if (sequence) return sequence;

            return GetComponents<EmoteSequenceSourceBase>() // Find in self
                .Concat(GetComponentsInParent<EmoteSequenceSourceBase>()) // then find in parents
                .Concat(GetComponentsInChildren<EmoteSequenceSourceBase>()) // then find in children
                .FirstOrDefault();
        }

        GenericEmoteItemTemplate ToTemplate()
        {
            IEmoteSequenceFactoryTemplate FindEmoteFactory()
            {
                var source = FindEmoteSequenceSource();
                if (!source) return null;

                return source.ToEmoteFactoryTemplate();
            }

            // TODO cache me?
            return new GenericEmoteItemTemplate(gameObject.name, handSign, FindEmoteFactory());
        }

        public IEnumerable<EmoteItem> ToEmoteItems() => ToTemplate().ToEmoteItems();

        public IEnumerable<ExpressionItem> ToExpressionItems() => ToTemplate().ToExpressionItems();
    }
}