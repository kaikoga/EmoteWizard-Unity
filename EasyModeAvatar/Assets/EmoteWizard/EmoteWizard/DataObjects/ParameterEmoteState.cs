using System;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterEmoteState
    {
        [SerializeField] public float value;
        [SerializeField] public AnimationClip clip;
    }
}