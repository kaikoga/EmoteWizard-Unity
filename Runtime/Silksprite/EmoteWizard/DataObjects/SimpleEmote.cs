using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Impl;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class SimpleEmote
    {
        [SerializeField] public LayerKind layerKind = LayerKind.FX;
        [SerializeField] public string groupName;

        [Header("Animation")]
        [SerializeField] public AnimatedEnable[] animatedEnable;
        
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

        [Serializable]
        public class AnimatedEnable
        {
            [SerializeField] public Transform target;
            [SerializeField] public bool isEnable;

            public SimpleEmoteFactory.AnimatedValue<float> ToAnimatedValue(Transform avatarRootTransform)
            {
                return new SimpleEmoteFactory.AnimatedValue<float>
                {
                    Path = target.GetRelativePathFrom(avatarRootTransform),
                    PropertyName = "m_IsActive",
                    Type = typeof(GameObject),
                    Value = isEnable ? 1 : 0
                };
            }
        }
    }
}