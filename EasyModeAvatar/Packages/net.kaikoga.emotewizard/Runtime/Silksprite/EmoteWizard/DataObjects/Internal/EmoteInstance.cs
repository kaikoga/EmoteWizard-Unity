using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class EmoteInstance
    {
        public readonly EmoteTrigger Trigger;
        public readonly EmoteSequence Sequence;

        public readonly EmoteHand Hand;

        public EmoteItemGroupInstance GroupInstance => new EmoteItemGroupInstance(Sequence.groupName, Hand);

        public EmoteInstance(EmoteTrigger trigger, EmoteSequence sequence, EmoteHand hand)
        {
            Trigger = trigger;
            Sequence = sequence;
            Hand = hand;
        }

        public readonly struct EmoteItemGroupInstance : IEquatable<EmoteItemGroupInstance>
        {
            readonly string _groupName;
            public readonly EmoteHand Hand;

            public string Name => Hand == EmoteHand.Neither ? _groupName : $"{_groupName} ({Hand})";

            public EmoteItemGroupInstance(string groupName, EmoteHand hand)
            {
                _groupName = groupName;
                Hand = hand;
            }

            #region IEquatable

            public bool Equals(EmoteItemGroupInstance other)
            {
                return _groupName == other._groupName && Hand == other.Hand;
            }

            public override bool Equals(object obj)
            {
                return obj is EmoteItemGroupInstance other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_groupName != null ? _groupName.GetHashCode() : 0) * 397) ^ (int)Hand;
                }
            }

            public static bool operator ==(EmoteItemGroupInstance left, EmoteItemGroupInstance right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(EmoteItemGroupInstance left, EmoteItemGroupInstance right)
            {
                return !left.Equals(right);
            }

            #endregion
        }
    }
}
