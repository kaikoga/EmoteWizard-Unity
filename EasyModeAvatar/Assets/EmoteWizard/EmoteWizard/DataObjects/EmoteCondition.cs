using System;
using UnityEditor.Animations;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteCondition
    {
        [SerializeField] public string parameter;
        [SerializeField] public AnimatorConditionMode mode;
        [SerializeField] public float threshold;
    }
}