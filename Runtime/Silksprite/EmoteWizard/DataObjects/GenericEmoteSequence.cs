using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Templates.Sequence;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class GenericEmoteSequence
    {
        [SerializeField] public LayerKind layerKind = LayerKind.FX;
        [SerializeField] public string groupName;

        [Header("Animation")]
        [SerializeField] public AnimatedEnable[] animatedEnable = { };
        [SerializeField] public AnimatedBlendShape[] animatedBlendShapes = { };
        
        [Header("Common Settings")]
        [SerializeField] public bool isFixedDuration;

        [SerializeField] public float entryTransitionDuration;
        [SerializeField] public float exitTransitionDuration = 0.25f;

        [Header("Layer Blend")]
        [SerializeField] public bool hasLayerBlend;
        [SerializeField] public float blendIn = 0.25f;
        [SerializeField] public float blendOut = 0.25f;
            
        [Header("Tracking Overrides")]
        [SerializeField] public bool hasTrackingOverrides;
        [SerializeField] public List<TrackingOverride> trackingOverrides = new();

        public IEnumerable<GenericEmoteSequenceFactory.AnimatedValue<float>> ToAnimatedFloats(Transform avatarRootTransform)
        {
            return Enumerable.Empty<IAnimatedProperty<float>>()
                .Concat(animatedEnable)
                .Concat(animatedBlendShapes)
                .SelectMany(prop => prop.ToAnimatedValues(avatarRootTransform));
        }

        public interface IAnimatedProperty<T>
        {
            IEnumerable<GenericEmoteSequenceFactory.AnimatedValue<T>> ToAnimatedValues(Transform avatarRootTransform);
        }

        [Serializable]
        public class AnimatedEnable : IAnimatedProperty<float>
        {
            [SerializeField] public Transform target;
            [SerializeField] public bool isEnable;

            IEnumerable<GenericEmoteSequenceFactory.AnimatedValue<float>> IAnimatedProperty<float>.ToAnimatedValues(Transform avatarRootTransform)
            {
                if (!target) yield break;
                yield return new GenericEmoteSequenceFactory.AnimatedValue<float>
                {
                    Path = target.GetRelativePathFrom(avatarRootTransform),
                    PropertyName = "m_IsActive",
                    Type = typeof(GameObject),
                    Value = isEnable ? 1 : 0
                };
            }
        }

        [Serializable]
        public class AnimatedBlendShape : IAnimatedProperty<float>
        {
            [SerializeField] public SkinnedMeshRenderer target;
            [SerializeField] public string blendShapeName;
            [Range(0, 100)]
            [SerializeField] public float value;

            IEnumerable<GenericEmoteSequenceFactory.AnimatedValue<float>> IAnimatedProperty<float>.ToAnimatedValues(Transform avatarRootTransform)
            {
                if (!target) yield break;
                yield return new GenericEmoteSequenceFactory.AnimatedValue<float>
                {
                    Path = target.transform.GetRelativePathFrom(avatarRootTransform),
                    PropertyName = $"blendShape.{blendShapeName}",
                    Type = typeof(SkinnedMeshRenderer),
                    Value = value
                };
            }
        }
    }
}