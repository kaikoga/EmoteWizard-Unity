using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Animations.Base
{
    [Serializable]
    public abstract class AnimatedPropertyBase<TTarget>
    {
        [SerializeField] public TTarget target;
    }
}