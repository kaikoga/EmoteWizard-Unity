using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class MotionExtension
    {
        public static IEnumerable<AnimationClip> GetClipsRec(this Motion motion)
        {
            switch (motion)
            {
                case AnimationClip clip:
                    yield return clip;
                    break;
                case BlendTree blendTree:
                    foreach (var child in blendTree.children)
                    {
                        foreach (var childClip in GetClipsRec(child.motion)) yield return childClip;
                    }
                    break;
            }
        }

        public static void SetLoopTimeRec(this Motion motion, bool value)
        {
            switch (motion)
            {
                case AnimationClip clip:
                    clip.SetLoopTime(value);
                    break;
                case BlendTree blendTree:
                {
                    foreach (var child in blendTree.children)
                    {
                        child.motion.SetLoopTimeRec(value);
                    }

                    break;
                }
            }
        }
    }
}