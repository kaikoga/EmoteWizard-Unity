using EmoteWizard.Base;
using UnityEngine;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class GestureWizard : AnimationWizardBase
    {
        [SerializeField] public Motion globalClip;
        [SerializeField] public Motion ambienceClip;
    }
}