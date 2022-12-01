using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Legacy
{
    [Serializable]
    public class ActionEmote
    {
        [SerializeField] public bool enabled = true;
        [SerializeField] public string name;
        [SerializeField] public int emoteIndex;
        [SerializeField] public bool hasExitTime;
        [SerializeField] public Motion entryClip;
        [SerializeField] public Motion clip;
        [SerializeField] public Motion exitClip;
        
        [SerializeField] public float blendIn = 0.25f;
        [SerializeField] public float blendOut = 0.25f;
        
        [SerializeField] public float entryTransitionDuration = 0.25f;
        [SerializeField] public float entryClipExitTime = 0.7f;
        [SerializeField] public float postEntryTransitionDuration = 0.25f;
        [SerializeField] public float clipExitTime = 0.7f;
        [SerializeField] public float exitTransitionDuration = 0.25f;
        [SerializeField] public float exitClipExitTime = 0.7f;
        [SerializeField] public float postExitTransitionDuration = 0.25f;
    }
}