using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteParameter
    {
        [SerializeField] public bool normalizedTimeEnabled;
        [SerializeField] public string normalizedTimeLeft;
        [SerializeField] public string normalizedTimeRight;
        
        public static EmoteParameter Populate(HandSign handSign)
        {
            return new EmoteParameter
            {
                normalizedTimeEnabled = handSign == HandSign.Fist,
                normalizedTimeLeft = "GestureLeftWeight",
                normalizedTimeRight = "GestureRightWeight"
            };
        }
    }
}