using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects.Builders;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteSequence
    {
        [SerializeField] public LayerKind layerKind = LayerKind.FX;
        [SerializeField] public string groupName;

        [SerializeField] public bool isFixedDuration;

        [SerializeField] public Motion clip;
        [SerializeField] public float entryTransitionDuration;
        [SerializeField] public float exitTransitionDuration = 0.25f;

        [SerializeField] public bool hasExitTime;
        [SerializeField] public float clipExitTime = 0.7f;

        [SerializeField] public bool hasTimeParameter;
        [ParameterName(false, false)]
        [SerializeField] public string timeParameter;

        [SerializeField] public bool hasEntryClip;
        [SerializeField] public Motion entryClip;
        [SerializeField] public float entryClipExitTime = 0.7f;
        [SerializeField] public float postEntryTransitionDuration = 0.25f;

        [SerializeField] public bool hasExitClip;
        [SerializeField] public Motion exitClip;
        [SerializeField] public float exitClipExitTime = 0.7f;
        [SerializeField] public float postExitTransitionDuration = 0.25f;
            
        [SerializeField] public bool hasLayerBlend;
        [SerializeField] public float blendIn = 0.25f;
        [SerializeField] public float blendOut = 0.25f;
            
        [SerializeField] public bool hasTrackingOverrides;
        [SerializeField] public List<TrackingOverride> trackingOverrides = new List<TrackingOverride>();
        
        public bool LooksLikeMirrorItem => hasTimeParameter && EmoteWizardConstants.Params.IsMirrorParameter(timeParameter);

        public static EmoteSequenceBuilder Builder(LayerKind layerKind, string groupName)
        {
            return new EmoteSequenceBuilder(new EmoteSequence
            {
                layerKind = layerKind,
                groupName = groupName
            });
        }
    }
}