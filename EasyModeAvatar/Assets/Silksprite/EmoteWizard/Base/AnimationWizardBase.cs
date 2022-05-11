using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimationWizardBase<TEmoteSource, TParameterEmoteSource, TAnimationMixinSource> : AnimationWizardBase
        where TEmoteSource : EmoteSourceBase
        where TParameterEmoteSource : ParameterEmoteSourceBase
        where TAnimationMixinSource : AnimationMixinSourceBase
    {
        public override IEnumerable<AnimationMixin> CollectBaseMixins()
        {
            return GetComponentsInChildren<TAnimationMixinSource>().SelectMany(source => source.baseMixins)
                .Where(m => m.enabled);
        }

        public override IEnumerable<Emote> CollectEmotes()
        {
            return GetComponentsInChildren<TEmoteSource>().SelectMany(source => source.emotes);
        }

        public override IEnumerable<ParameterEmote> CollectParameterEmotes(bool includeInactive = false, bool includeDisabled = false)
        {
            return GetComponentsInChildren<TParameterEmoteSource>(includeInactive).SelectMany(source => source.parameterEmotes)
                .Where(e => e.enabled || includeDisabled);
        }

        public override IEnumerable<AnimationMixin> CollectMixins()
        {
            return GetComponentsInChildren<TAnimationMixinSource>().SelectMany(source => source.mixins)
                .Where(m => m.enabled);
        }
    }
    
    public abstract class AnimationWizardBase : EmoteWizardBase
    {
        [SerializeField] public bool advancedAnimations;
        [SerializeField] public bool handSignOverrideEnabled;

        [FormerlySerializedAs("baseMixins")]
        [SerializeField] public List<AnimationMixin> legacyBaseMixins;
        [FormerlySerializedAs("emotes")]
        [SerializeField] public List<Emote> legacyEmotes;
        [FormerlySerializedAs("parameterEmotes")]
        [SerializeField] public List<ParameterEmote> legacyParameterEmotes;
        [FormerlySerializedAs("mixins")]
        [SerializeField] public List<AnimationMixin> legacyMixins;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        public abstract IEnumerable<AnimationMixin> CollectBaseMixins();
        public abstract IEnumerable<Emote> CollectEmotes();
        public abstract IEnumerable<ParameterEmote> CollectParameterEmotes(bool includeInactive = false, bool includeDisabled = false);
        public abstract IEnumerable<AnimationMixin> CollectMixins();

        public abstract string LayerName { get; }
        public abstract string HandSignOverrideParameter { get;  }

        public bool HasLegacyData => legacyBaseMixins.Any() || legacyEmotes.Any() || legacyParameterEmotes.Any() || legacyMixins.Any();
    }
}