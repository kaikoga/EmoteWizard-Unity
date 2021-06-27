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

        public string ToStateName() => BuildStateName(gesture1.mode, gesture1.handSign, gesture2.mode, gesture2.handSign);

        public static string BuildStateName(GestureConditionMode mode1, EmoteHandSign handSign1, GestureConditionMode mode2, EmoteHandSign handSign2)
        {
            string ToPart(GestureConditionMode mode, EmoteHandSign handSign)
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
                if (mode1 != GestureConditionMode.Ignore) yield return ToPart(mode1, handSign1);
                if (mode2 != GestureConditionMode.Ignore) yield return ToPart(mode2, handSign2);
            }

            return string.Join(" ", ToParts());
        }
    }
}