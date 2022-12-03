using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteItem
    {
        [SerializeField] public EmoteTrigger trigger;
        [SerializeField] public EmoteSequence sequence;

        [SerializeField] public EmoteHand hand;

        public string GroupInstanceName => hand == EmoteHand.Neither ? trigger.groupName : $"{trigger.groupName} ({hand})";

        public EmoteItem(EmoteTrigger trigger, EmoteSequence sequence, EmoteHand hand)
        {
            this.trigger = trigger;
            this.sequence = sequence;
            this.hand = hand;
        }
    }
}
