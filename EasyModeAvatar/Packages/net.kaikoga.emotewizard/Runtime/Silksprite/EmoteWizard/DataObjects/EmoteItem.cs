using System;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteItem
    {
        [SerializeField] public EmoteTrigger trigger;
        [SerializeField] public EmoteSequence sequence;

        [SerializeField] public EmoteHand hand;

        public EmoteItem(EmoteTrigger trigger, EmoteSequence sequence, EmoteHand hand)
        {
            this.trigger = trigger;
            this.sequence = sequence;
            this.hand = hand;
        }

        public EmoteInstance ToInstance()
        {
            return new EmoteInstance(trigger, sequence, hand);
        }
    }
}
