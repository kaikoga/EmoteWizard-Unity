using System;
using UnityEditor.Animations;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class AnimationMixin
    {
        [SerializeField] public string name;
        [SerializeField] public AnimationMixinKind kind = AnimationMixinKind.AnimationClip;
        [SerializeField] public AnimationClip animationClip;
        [SerializeField] public EmoteControl control;

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
    }
}