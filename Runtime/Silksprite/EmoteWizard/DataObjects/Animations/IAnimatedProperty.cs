using System.Collections.Generic;
using Silksprite.EmoteWizard.ClipBuilder;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Animations
{
    public interface IAnimatedProperty<T>
    {
        IEnumerable<AnimatedValue<T>> ToAnimatedValues(Transform avatarRootTransform);
    }
}