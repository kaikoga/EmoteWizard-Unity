using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimationWizardBase<TEmoteSource, TParameterEmoteSource, TAnimationMixinSource> : AnimationWizardBase
        where TEmoteSource : IEmoteSourceBase
        where TParameterEmoteSource : IParameterEmoteSourceBase
        where TAnimationMixinSource : IAnimationMixinSourceBase
    {
        public virtual IEnumerable<Emote> CollectEmotes()
        {
            return EmoteWizardRoot.GetComponentsInChildren<TEmoteSource>().SelectMany(source => source.Emotes);
        }
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