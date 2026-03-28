using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public struct ParameterReadUsage
    {
        [SerializeField] public ParameterItemKind itemKind;
        [SerializeField] float value;

        [Obsolete]
        public float ForcedFloatValueUnsafe => value;

        public bool IsDefault => value == 0;

        public ParameterReadUsage(ParameterItemKind itemKind, float value)
        {
            this.itemKind = itemKind;
            this.value = value;
        }
    }
}