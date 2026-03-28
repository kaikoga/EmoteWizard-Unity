using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public struct ParameterReadUsage
    {
        [SerializeField] public ParameterItemKind itemKind;
        [SerializeField] float value;

        public ParameterValue Value => ParameterValue.CreateInstance(itemKind, value);

        public ParameterReadUsage(ParameterItemKind itemKind, float value)
        {
            this.itemKind = itemKind;
            this.value = value;
        }
    }
}