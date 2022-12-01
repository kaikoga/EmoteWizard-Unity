using System;
using System.Collections.Generic;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteTrigger
    {
        [SerializeField] public string name;
        [SerializeField] public LayerKind layerKind;
        [SerializeField] public string groupName;
        [SerializeField] public int priority;
        [SerializeField] public List<EmoteCondition> conditions = new List<EmoteCondition>();
    }
}