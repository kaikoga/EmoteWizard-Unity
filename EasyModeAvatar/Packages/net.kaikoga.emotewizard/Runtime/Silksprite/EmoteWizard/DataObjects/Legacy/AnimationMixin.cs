using System;
using System.Collections.Generic;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Legacy
{
    [Serializable]
    public class AnimationMixin
    {
        [SerializeField] public bool enabled = true;
        [SerializeField] public string name;
        [SerializeField] public AnimationMixinKind kind = AnimationMixinKind.AnimationClip;
        [SerializeField] public AnimationClip animationClip;
        [SerializeField] public EmoteControl control;
        [SerializeField] public Motion blendTree;

        [SerializeField] public List<EmoteCondition> conditions = new List<EmoteCondition>();

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