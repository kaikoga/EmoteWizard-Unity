using EmoteWizard.Base;
using UnityEngine;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class FxWizard : AnimationWizardBase
    {
        [SerializeField] public AnimationClip resetClip;
        [SerializeField] public Motion globalClip;
        [SerializeField] public Motion ambienceClip;
        public override string LayerName => "FX";
    }
}