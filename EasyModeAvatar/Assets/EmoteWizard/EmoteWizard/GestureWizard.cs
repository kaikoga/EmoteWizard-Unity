using EmoteWizard.Base;
using UnityEngine;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class GestureWizard : AnimationWizardBase
    {
        [SerializeField] public AnimationClip globalClip;
        [SerializeField] public AnimationClip ambienceClip;
    }
}