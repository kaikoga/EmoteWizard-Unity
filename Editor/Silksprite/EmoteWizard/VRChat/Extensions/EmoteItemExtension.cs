using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects.Internal;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class EmoteItemExtension
    {
        public static IEnumerable<AnimationClip> AllClipsRec(this EmoteItem emoteItem)
        {
            return emoteItem.AllClipRefs()
                .SelectMany(clip => clip.GetClipsRec())
                .Where(c => c != null).Distinct().ToList();
        }
    }
}