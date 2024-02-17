using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.ClipBuilder;
using Silksprite.EmoteWizard.DataObjects.Animations.Base;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Animations
{
    [Serializable]
    public class AnimatedBlendShape : AnimatedPropertyBase<SkinnedMeshRenderer>, IAnimatedProperty<float>
    {
        [SerializeField] public string blendShapeName;
        [Range(0, 100)]
        [SerializeField] public float value;

        IEnumerable<AnimatedValue<float>> IAnimatedProperty<float>.ToAnimatedValues(Transform avatarRootTransform)
        {
            if (!target) yield break;
            yield return new AnimatedValue<float>
            {
                Path = RuntimeUtil.CalculateAnimationTransformPath(avatarRootTransform, target.transform),
                PropertyName = $"blendShape.{blendShapeName}",
                Type = typeof(SkinnedMeshRenderer),
                ValueOff = value,
                ValueOn = value
            };
        }
    }
}