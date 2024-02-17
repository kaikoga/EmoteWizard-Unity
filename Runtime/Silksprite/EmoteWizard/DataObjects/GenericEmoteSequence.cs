using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.ClipBuilder;
using Silksprite.EmoteWizard.DataObjects.Animations;
using Silksprite.EmoteWizard.DataObjects.Builders;
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

        [SerializeField] public bool hasTimeParameter;
        [ParameterName(false, false)]
        [SerializeField] public string timeParameter;

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
    }
}