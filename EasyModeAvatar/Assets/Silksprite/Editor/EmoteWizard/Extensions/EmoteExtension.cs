using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class EmoteExtension
    {
        public static IEnumerable<AnimationClip> AllClips(this Emote emote)
        {
            if (emote.clipLeft != null) foreach (var clip in emote.clipLeft.GetClipsRec()) yield return clip;
            if (emote.clipRight != null) foreach (var clip in emote.clipRight.GetClipsRec()) yield return clip;
        }
    }
}