using System;
using System.Collections.Generic;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class EmoteSequence
    {
        [SerializeField] public LayerKind layerKind;
        [SerializeField] public string groupName;

        [Header("Common Settings")]
        [SerializeField] public bool isFixedDuration;

        [SerializeField] public Motion clip;
        [SerializeField] public float entryTransitionDuration;
        [SerializeField] public float exitTransitionDuration = 0.25f;

        [Header("Exit Time")]
        [SerializeField] public bool hasExitTime;
        [SerializeField] public float clipExitTime = 0.7f;

        [Header("Time Parameter")]
        [SerializeField] public bool hasTimeParameter;
        [SerializeField] public string timeParameter;

        [Header("Entry Clip")]
        [SerializeField] public bool hasEntryClip;
        [SerializeField] public Motion entryClip;
        [SerializeField] public float entryClipExitTime = 0.7f;
        [SerializeField] public float postEntryTransitionDuration = 0.25f;

        [Header("Exit Clip")]
        [SerializeField] public bool hasExitClip;
        [SerializeField] public Motion exitClip;
        [SerializeField] public float exitClipExitTime = 0.7f;
        [SerializeField] public float postExitTransitionDuration = 0.25f;
            
        [Header("Layer Blend")]
        [SerializeField] public bool hasLayerBlend;
        [SerializeField] public float blendIn = 0.25f;
        [SerializeField] public float blendOut = 0.25f;
            
        [Header("Tracking Overrides")]
        [SerializeField] public bool hasTrackingOverrides;
        [SerializeField] public List<TrackingOverride> trackingOverrides = new List<TrackingOverride>();
    }
}