using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
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
                parameter = EmoteParameter.Populate(handSign)
            };
        }
        
        [SerializeField] public EmoteGestureCondition gesture1;
        [SerializeField] public EmoteGestureCondition gesture2;
        [SerializeField] public List<EmoteCondition> conditions = new List<EmoteCondition>();
        [SerializeField] public Motion clipLeft;
        [SerializeField] public Motion clipRight;
        [SerializeField] public EmoteParameter parameter;

        public IEnumerable<AnimationClip> AllClips()
        {
            IEnumerable<AnimationClip> CollectClips(Motion motion)
            {
                switch (motion)
                {
                    case AnimationClip clip:
                        yield return clip;
                        break;
                    case BlendTree blendTree:
                        foreach (var child in blendTree.children)
                        {
                            foreach (var childClip in CollectClips(child.motion)) yield return childClip;
                        }
                        break;
                }
            }
            if (clipLeft != null) foreach (var clip in CollectClips(clipLeft)) yield return clip;
            if (clipRight != null) foreach (var clip in CollectClips(clipRight)) yield return clip;
        }

        public string ToStateName() => BuildStateName(gesture1.mode, gesture1.handSign, gesture2.mode, gesture2.handSign);

        public static string BuildStateName(GestureConditionMode mode1, HandSign handSign1, GestureConditionMode mode2, HandSign handSign2)
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
                if (mode1 != GestureConditionMode.Ignore) yield return ToPart(mode1, handSign1);
                if (mode2 != GestureConditionMode.Ignore) yield return ToPart(mode2, handSign2);
            }

            return string.Join(" ", ToParts());
        }
    }
}