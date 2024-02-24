using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public struct ParameterReadUsage
    {
        [SerializeField] public ParameterItemKind ItemKind;
        [SerializeField] public float Value;

        public ParameterReadUsage(ParameterItemKind itemKind, float value)
        {
            ItemKind = itemKind;
            Value = value;
        }
    }
}