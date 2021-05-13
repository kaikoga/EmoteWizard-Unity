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
    }
}