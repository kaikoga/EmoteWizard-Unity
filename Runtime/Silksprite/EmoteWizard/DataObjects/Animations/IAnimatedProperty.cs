using System.Collections.Generic;
using Silksprite.EmoteWizardSupport.ClipBuilder;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Animations
{
    public interface IAnimatedProperty<T>
    {
        IEnumerable<AnimatedValue<T>> ToAnimatedValues(Transform avatarRootTransform);
    }
}