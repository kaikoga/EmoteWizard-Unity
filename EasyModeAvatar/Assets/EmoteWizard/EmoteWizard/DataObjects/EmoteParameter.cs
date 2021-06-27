using System;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteParameter
    {
        [SerializeField] public bool normalizedTimeEnabled;
        [SerializeField] public string normalizedTimeLeft;
        [SerializeField] public string normalizedTimeRight;
        
        public static EmoteParameter Populate(EmoteHandSign handSign)
        {
            return new EmoteParameter
            {
                normalizedTimeEnabled = handSign == EmoteHandSign.Fist,
                normalizedTimeLeft = "GestureLeftWeight",
                normalizedTimeRight = "GestureRightWeight"
            };
        }
    }
}