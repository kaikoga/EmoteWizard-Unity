using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class Emote
    {
        public static IEnumerable<EmoteHandSign> HandSigns =>
            Enum.GetValues(typeof(EmoteHandSign)).OfType<EmoteHandSign>();

        public static Emote Populate(EmoteHandSign handSign)
        {
            return new Emote
            {
                gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther, GestureConditionMode.Ignore)
            };
        }
        
        [SerializeField] public EmoteGestureCondition gesture1;
        [SerializeField] public EmoteGestureCondition gesture2;
        [SerializeField] public List<EmoteCondition> conditions = new List<EmoteCondition>();
        [SerializeField] public AnimationClip clipLeft;
        [SerializeField] public AnimationClip clipRight;
        [SerializeField] public EmoteParameter parameter;

        public IEnumerable<AnimationClip> AllClips()
        {
            if (clipLeft != null) yield return clipLeft;
            if (clipRight != null) yield return clipRight;
        }

        public string ToStateName()
        {
            string ToPart(EmoteGestureCondition gesture)
            {
                switch (gesture.mode)
                {
                    case GestureConditionMode.Equals:
                        return $"{gesture.handSign}";
                    case GestureConditionMode.NotEqual:
                        return $"¬{gesture.handSign}";
                    case GestureConditionMode.Ignore:
                        return "";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            IEnumerable<string> ToParts()
            {
                if (gesture1.mode != GestureConditionMode.Ignore) yield return ToPart(gesture1);
                if (gesture2.mode != GestureConditionMode.Ignore) yield return ToPart(gesture2);
            }

            return string.Join(" ", ToParts());
        }
    }
}