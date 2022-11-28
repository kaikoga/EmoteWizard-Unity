using Silksprite.EmoteWizard.Sources.Legacy.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base.Legacy
{
    public abstract class AnimationWizardBase<TEmoteSource, TParameterEmoteSource, TAnimationMixinSource> : AnimationWizardBase
        where TEmoteSource : IEmoteSourceBase
        where TParameterEmoteSource : IParameterEmoteSourceBase
        where TAnimationMixinSource : IAnimationMixinSourceBase
    {
    }
    
    public abstract class AnimationWizardBase : EmoteWizardBase
    {
        [SerializeField] public bool handSignOverrideEnabled;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        public override void DisconnectOutputAssets()
        {
            outputAsset = null;
        }
    }
}