using System;
using System.Collections.Generic;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Legacy
{
    [Serializable]
    public class EmoteControl
    {
        [SerializeField] public float transitionDuration = 0.1f;
        [SerializeField] public bool normalizedTimeEnabled;
        [SerializeField] public string normalizedTimeLeft = "GestureLeftWeight";
        [SerializeField] public string normalizedTimeRight = "GestureRightWeight";
        [SerializeField] public List<TrackingOverride> trackingOverrides = new List<TrackingOverride>();

        public static EmoteControl Populate(HandSign handSign)
        {
            return new EmoteControl
            {
                normalizedTimeEnabled = handSign == HandSign.Fist,
            };
        }
    }
}