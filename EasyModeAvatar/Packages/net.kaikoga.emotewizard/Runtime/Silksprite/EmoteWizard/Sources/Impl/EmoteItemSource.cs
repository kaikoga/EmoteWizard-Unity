using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class EmoteItemSource : EmoteWizardDataSourceBase, IEmoteItemSource
    {
        [SerializeField] public EmoteTrigger trigger;

        public bool IsMirrorItem
        {
            get
            {
                var sequence = GetComponentsInParent<EmoteSequenceSourceBase>().Select(source => source.EmoteSequence).FirstOrDefault();
                if (sequence == null) return false;

                return new EmoteItem(trigger, sequence).IsMirrorItem;
            }
        }

        public IEnumerable<EmoteItem> EmoteItems
        {
            get
            {
                var sequence = GetComponentsInParent<EmoteSequenceSourceBase>().Select(source => source.EmoteSequence).FirstOrDefault();
                if (sequence == null) yield break;

                var emoteItem = new EmoteItem(trigger, sequence);
                if (emoteItem.IsMirrorItem)
                {
                    yield return emoteItem.Mirror("Left");
                    yield return emoteItem.Mirror("Right");
                }
                else
                {
                    yield return emoteItem;
                }
            }
        }
    }
}