using System.Collections.Generic;
using EmoteWizard.DataObjects;
using UnityEditor.Animations;
using UnityEngine;

namespace EmoteWizard.Base
{
    [RequireComponent(typeof(EmoteWizardRoot))]
    public abstract class AnimationWizardBase : MonoBehaviour
    {
        [SerializeField] public List<Emote> emotes;

        [SerializeField] public AnimatorController outputAsset;

        public EmoteWizardRoot EmoteWizardRoot => GetComponent<EmoteWizardRoot>();
    }
}