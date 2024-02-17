using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.ClipBuilder;
using Silksprite.EmoteWizard.DataObjects.Animations.Base;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Animations
{
    [Serializable]
    public class AnimatedEnable : AnimatedPropertyBase<Transform>, IAnimatedProperty<float>
    {
        [SerializeField] public bool isEnable;

        IEnumerable<AnimatedValue<float>> IAnimatedProperty<float>.ToAnimatedValues(Transform avatarRootTransform)
        {
            if (!target) yield break;
            yield return new AnimatedValue<float>
            {
                Path = RuntimeUtil.CalculateAnimationTransformPath(avatarRootTransform, target),
                PropertyName = "m_IsActive",
                Type = typeof(GameObject),
                ValueOff = isEnable ? 1 : 0,
                ValueOn = isEnable ? 1 : 0,
            };
        }
    }
}