using System.Collections.Generic;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Impl
{
    public class EmoteItemSource : EmoteWizardDataSourceBase, IEmoteItemSource
    {
        [SerializeField] public EmoteTrigger trigger;
        [SerializeField] public bool mirror;

        public IEnumerable<EmoteItem> EmoteItems
        {
            get
            {
                var sequence = GetComponentInParent<EmoteSequenceSourceBase>()?.EmoteSequence;
                if (sequence == null) yield break;

                var emoteItem = new EmoteItem(trigger, sequence);
                if (mirror)
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