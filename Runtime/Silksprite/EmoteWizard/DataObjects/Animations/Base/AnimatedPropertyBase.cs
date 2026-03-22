using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Animations.Base
{
    [Serializable]
    public abstract class AnimatedPropertyBase<TTarget, TRelativeRef>
    where TTarget : Component
    where TRelativeRef : RelativeRef<TTarget>, new()
    {
        [SerializeField] public TRelativeRef relativeRef = new TRelativeRef();
    }
}