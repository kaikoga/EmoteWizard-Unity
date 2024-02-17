using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.ClipBuilder;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Animations
{
    [Serializable]
    public class AnimatedBlendShape : IAnimatedProperty<float>
    {
        [SerializeField] public SkinnedMeshRenderer target;
        [SerializeField] public string blendShapeName;
        [Range(0, 100)]
        [SerializeField] public float value;

        IEnumerable<AnimatedValue<float>> IAnimatedProperty<float>.ToAnimatedValues(Transform avatarRootTransform)
        {
            if (!target) yield break;
            yield return new AnimatedValue<float>
            {
                Path = target.transform.GetRelativePathFrom(avatarRootTransform),
                PropertyName = $"blendShape.{blendShapeName}",
                Type = typeof(SkinnedMeshRenderer),
                ValueOff = value,
                ValueOn = value
            };
        }
    }
}