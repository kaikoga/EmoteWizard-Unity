using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class Emote
    {
        public static IEnumerable<HandSign> HandSigns =>
            Enum.GetValues(typeof(HandSign)).OfType<HandSign>();

        public static Emote Populate(HandSign handSign)
        {
            return new Emote
            {
                gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther, GestureConditionMode.Ignore),
                control = EmoteControl.Populate(handSign)
            };
        }
        
        [SerializeField] public EmoteGestureCondition gesture1;
        [SerializeField] public EmoteGestureCondition gesture2;
        [SerializeField] public List<EmoteCondition> conditions = new List<EmoteCondition>();
        [SerializeField] public bool overrideEnabled;
        [SerializeField] public int overrideIndex;
        [SerializeField] public Motion clipLeft;
        [SerializeField] public Motion clipRight;
        [SerializeField] public EmoteControl control;

        public bool OverrideAvailable => overrideEnabled && overrideIndex > 0;

        public string ToStateName()
        {
            string ToPart(GestureConditionMode mode, HandSign handSign)
            {
                switch (mode)
                {
                    case GestureConditionMode.Equals:
                        return $"{handSign}";
                    case GestureConditionMode.NotEqual:
                        return $"¬{handSign}";
                    case GestureConditionMode.Ignore:
                        return "";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            IEnumerable<string> ToParts()
            {
                if (gesture1.mode != GestureConditionMode.Ignore) yield return ToPart(gesture1.mode, gesture1.handSign);
                if (gesture2.mode != GestureConditionMode.Ignore) yield return ToPart(gesture2.mode, gesture2.handSign);
                if (OverrideAvailable) yield return $"({overrideIndex})";
            }

            return string.Join(" ", ToParts());
        }
    }
}