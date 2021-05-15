using System.Collections.Generic;
using EmoteWizard.DataObjects;
using UnityEditor.Animations;
using UnityEngine;

namespace EmoteWizard.Base
{
    public abstract class AnimationWizardBase : EmoteWizardBase
    {
        [SerializeField] public List<Emote> emotes;

        [SerializeField] public AnimatorController outputAsset;
    }
}