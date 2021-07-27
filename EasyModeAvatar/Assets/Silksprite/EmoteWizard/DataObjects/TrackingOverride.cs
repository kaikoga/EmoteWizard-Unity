using System;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class TrackingOverride
    {
        [SerializeField] public TrackingTarget target;
        [SerializeField] public TrackingType type;

        public enum TrackingTarget
        {
            Head,
            LeftHand,
            RightHand,
            Hip,
            LeftFoot,
            RightFoot,
            LeftFingers,
            RightFingers,
            Eyes,
            Mouth
        }

        public enum TrackingType
        {
            NoChange,
            Tracking,
            Animation,
        }
    }
}