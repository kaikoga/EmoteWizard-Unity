using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Animations.Base
{
    [Serializable]
    public abstract class AnimatedPropertyBase<TTarget, TRelativeRef>
    where TTarget : Component
    where TRelativeRef : RelativeRef<TTarget>
    {
        [SerializeField] public TRelativeRef relativeRef;
    }
}