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
        public override IEnumerable<AnimationMixin> CollectBaseMixins()
        {
            return EmoteWizardRoot.GetComponentsInChildren<TAnimationMixinSource>().SelectMany(source => source.BaseMixins)
                .Where(m => m.enabled);
        }

        public override IEnumerable<Emote> CollectEmotes()
        {
            return EmoteWizardRoot.GetComponentsInChildren<TEmoteSource>().SelectMany(source => source.Emotes);
        }

        public override IEnumerable<ParameterEmote> CollectParameterEmotes(bool includeInactive = false, bool includeDisabled = false)
        {
            return EmoteWizardRoot.GetComponentsInChildren<TParameterEmoteSource>(includeInactive).SelectMany(source => source.ParameterEmotes)
                .Where(e => e.enabled || includeDisabled);
        }

        public override IEnumerable<AnimationMixin> CollectMixins()
        {
            return EmoteWizardRoot.GetComponentsInChildren<TAnimationMixinSource>().SelectMany(source => source.Mixins)
                .Where(m => m.enabled);
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

        public abstract IEnumerable<AnimationMixin> CollectBaseMixins();
        public abstract IEnumerable<Emote> CollectEmotes();
        public abstract IEnumerable<ParameterEmote> CollectParameterEmotes(bool includeInactive = false, bool includeDisabled = false);
        public abstract IEnumerable<AnimationMixin> CollectMixins();

        public abstract string LayerName { get; }
        public abstract string HandSignOverrideParameter { get;  }
    }
}