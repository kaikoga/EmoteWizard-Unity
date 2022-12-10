using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class EmoteItemSource : EmoteWizardDataSourceBase, IEmoteItemSource
    {
        [SerializeField] public EmoteTrigger trigger;

        EmoteSequence FindEmoteSequence() => GetComponentsInParent<EmoteSequenceSourceBase>().Select(source => source.EmoteSequence).FirstOrDefault();

        EmoteItem ToEmoteItem()
        {
            var sequence = FindEmoteSequence();
            if (sequence == null) return null;

            return new EmoteItem(trigger, sequence);
        }

        public bool IsMirrorItem
        {
            get
            {
                var item = ToEmoteItem();
                if (item == null) return false;

                return item.IsMirrorItem;
            }
        }

        public IEnumerable<EmoteItem> EmoteItems
        {
            get
            {
                var emoteItem = ToEmoteItem();
                if (emoteItem != null) yield return emoteItem;
            }
        }

    }
}