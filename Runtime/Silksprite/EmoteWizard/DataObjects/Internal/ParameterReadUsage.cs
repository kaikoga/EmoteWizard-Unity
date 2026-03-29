using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public struct ParameterReadUsage
    {
        [SerializeField] ParameterValue value;

        public ParameterValue Value => value;

        public ParameterReadUsage(ParameterItemKind itemKind, float value)
        {
            this.value = ParameterValue.Create(itemKind, value);
        }

        public ParameterReadUsage(ParameterValue value)
        {
            this.value = value;
        }
    }
}