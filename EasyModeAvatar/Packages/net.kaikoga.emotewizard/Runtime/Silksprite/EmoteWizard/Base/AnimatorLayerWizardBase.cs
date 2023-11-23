using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimatorLayerWizardBase : EmoteWizardBase
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public AnimationClip resetClip;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        public abstract bool HasResetClip { get; }
        public abstract LayerKind LayerKind { get; }

        public override IBehaviourContext ToContext() => GetContext();
        public abstract AnimatorLayerContextBase GetContext();
    }
}