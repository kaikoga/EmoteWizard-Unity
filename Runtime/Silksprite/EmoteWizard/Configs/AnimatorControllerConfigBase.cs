using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Configs
{
    public abstract class AnimatorControllerConfigBase : EmoteConfigBase
    {
        [SerializeField] public AvatarMask defaultAvatarMask;

        [SerializeField] public AnimationClip resetClip;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        [SerializeField] public bool hasResetClip = false;
        public abstract LayerOutputKind LayerOutputKind { get; }

        public override IBehaviourContext ToContext(EmoteWizardEnvironment env) => GetContext(env);
        public abstract AnimatorControllerContextBase GetContext(EmoteWizardEnvironment env);

        protected virtual void Reset()
        {
            var context = GetContext(CreateEnv());
            hasResetClip = context.HasResetClip;
        }
    }
}