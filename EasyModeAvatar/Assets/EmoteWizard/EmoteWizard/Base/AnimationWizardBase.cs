using UnityEditor.Animations;
using UnityEngine;

namespace EmoteWizard.Base
{
    [RequireComponent(typeof(EmoteWizardRoot))]
    public abstract class AnimationWizardBase : MonoBehaviour
    {
        [SerializeField] AnimatorController outputAsset;
    }
}