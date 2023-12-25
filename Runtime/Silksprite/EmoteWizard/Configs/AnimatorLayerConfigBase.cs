using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimatorLayerConfigBase : EmoteConfigBase
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public AnimationClip resetClip;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        [SerializeField] public bool hasResetClip = false;
        public abstract LayerKind LayerKind { get; }

        public override IBehaviourContext ToContext(EmoteWizardEnvironment env) => GetContext(env);
        public abstract AnimatorLayerContextBase GetContext(EmoteWizardEnvironment env);

        protected virtual void Reset()
        {
            var context = GetContext(CreateEnv());
            hasResetClip = context.HasResetClip;
        }
    }
}