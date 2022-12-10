using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class EmoteItemExtension
    {
        public static IEnumerable<AnimationClip> AllClips(this EmoteItem emoteItem)
        {
            var sequence = emoteItem.Sequence;
            return Enumerable.Empty<AnimationClip>()
                .Concat(sequence.clip.GetClipsRec())
                .Concat((sequence.hasEntryClip ? sequence.entryClip : null).GetClipsRec())
                .Concat((sequence.hasExitClip ? sequence.exitClip : null).GetClipsRec());
        }
    }
}