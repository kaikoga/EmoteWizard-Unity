using System;
using System.Collections.Generic;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterEmoteState
    {
        [SerializeField] public float value;
        [SerializeField] public AnimationClip clip;
        [SerializeField] public List<GameObject> targets;
    }
}