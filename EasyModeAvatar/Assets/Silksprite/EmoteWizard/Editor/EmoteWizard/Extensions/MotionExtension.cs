using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class MotionExtension
    {
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