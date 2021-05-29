using System;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterState
    {
        public float value;
        public AnimationClip gestureClip;
        public AnimationClip fxClip;
    }
}