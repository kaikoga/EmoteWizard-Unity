using System;
using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class AnimationMixinExtension
    {
        public static IEnumerable<AnimationClip> AllClips(this AnimationMixin mixin)
        {
            switch (mixin.kind)
            {
                case AnimationMixinKind.AnimationClip:
                    yield return mixin.animationClip;
                    break;
                case AnimationMixinKind.BlendTree:
                    foreach (var clip in mixin.blendTree.GetClipsRec()) yield return clip; 
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}