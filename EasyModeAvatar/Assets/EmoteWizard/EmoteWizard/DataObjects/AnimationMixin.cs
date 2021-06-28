using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class AnimationMixin
    {
        [SerializeField] public string name;
        [SerializeField] public AnimationMixinKind kind = AnimationMixinKind.AnimationClip;
        [SerializeField] public AnimationClip animationClip;
        [SerializeField] public BlendTree blendTree;

        public Motion Motion
        {
            get
            {
                switch (kind)
                {
                    case AnimationMixinKind.AnimationClip:
                        return animationClip;
                    case AnimationMixinKind.BlendTree:
                        return blendTree;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public enum AnimationMixinKind
        {
            AnimationClip,
            BlendTree
        }
    }
}