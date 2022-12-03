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

        EmoteItemTemplate ToEmoteItemTemplate()
        {
            var sequence = FindEmoteSequence();
            if (sequence == null) return null;

            return new EmoteItemTemplate(trigger, sequence);
        }

        public bool IsMirrorItem
        {
            get
            {
                var template = ToEmoteItemTemplate();
                if (template == null) return false;

                return template.IsMirrorItem;
            }
        }

        public IEnumerable<EmoteItem> EmoteItems
        {
            get
            {
                var template = ToEmoteItemTemplate();
                if (template == null) yield break;

                if (template.IsMirrorItem)
                {
                    yield return template.Mirror(EmoteHand.Left);
                    yield return template.Mirror(EmoteHand.Right);
                }
                else
                {
                    yield return template.ToEmoteItem();
                }
            }
        }

    }
}