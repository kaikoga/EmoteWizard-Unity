using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteControl
    {
        [SerializeField] public float transitionDuration = 0.1f;
        [SerializeField] public bool normalizedTimeEnabled;
        [SerializeField] public string normalizedTimeLeft;
        [SerializeField] public string normalizedTimeRight;
        
        public static EmoteControl Populate(HandSign handSign)
        {
            return new EmoteControl
            {
                transitionDuration = 0.1f,
                normalizedTimeEnabled = handSign == HandSign.Fist,
                normalizedTimeLeft = "GestureLeftWeight",
                normalizedTimeRight = "GestureRightWeight"
            };
        }
    }
}