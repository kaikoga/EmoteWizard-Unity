using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.ClipBuilder;
using Silksprite.EmoteWizard.DataObjects.Builders;
using Silksprite.EmoteWizard.Templates.Impl.Builders;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class GenericEmoteSequence
    {
        [SerializeField] public LayerKind layerKind = LayerKind.FX;
        [SerializeField] public string groupName;

        [SerializeField] public AnimatedEnable[] animatedEnable = { };
        [SerializeField] public AnimatedBlendShape[] animatedBlendShapes = { };
        
        [SerializeField] public bool isFixedDuration;

        [SerializeField] public float entryTransitionDuration;
        [SerializeField] public float exitTransitionDuration = 0.25f;

        [SerializeField] public bool hasLayerBlend;
        [SerializeField] public float blendIn = 0.25f;
        [SerializeField] public float blendOut = 0.25f;
            
        [SerializeField] public bool hasTrackingOverrides;
        [SerializeField] public List<TrackingOverride> trackingOverrides = new List<TrackingOverride>();

        public IEnumerable<AnimatedValue<float>> ToAnimatedFloats(Transform avatarRootTransform)
        {
            return Enumerable.Empty<IAnimatedProperty<float>>()
                .Concat(animatedEnable)
                .Concat(animatedBlendShapes)
                .SelectMany(prop => prop.ToAnimatedValues(avatarRootTransform));
        }

        public static GenericEmoteSequenceBuilder Builder(LayerKind layerKind, string groupName)
        {
            return new GenericEmoteSequenceBuilder(new GenericEmoteSequence
            {
                layerKind = layerKind,
                groupName = groupName
            });
        }

        public interface IAnimatedProperty<T>
        {
            IEnumerable<AnimatedValue<T>> ToAnimatedValues(Transform avatarRootTransform);
        }

        [Serializable]
        public class AnimatedEnable : IAnimatedProperty<float>
        {
            [SerializeField] public Transform target;
            [SerializeField] public bool isEnable;

            IEnumerable<AnimatedValue<float>> IAnimatedProperty<float>.ToAnimatedValues(Transform avatarRootTransform)
            {
                if (!target) yield break;
                yield return new AnimatedValue<float>
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

            IEnumerable<AnimatedValue<float>> IAnimatedProperty<float>.ToAnimatedValues(Transform avatarRootTransform)
            {
                if (!target) yield break;
                yield return new AnimatedValue<float>
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