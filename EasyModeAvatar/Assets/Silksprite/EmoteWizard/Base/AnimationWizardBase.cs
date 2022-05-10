using System;
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

        public override IEnumerable<ParameterEmote> CollectParameterEmotes()
        {
            return GetComponentsInChildren<TParameterEmoteSource>().SelectMany(source => source.parameterEmotes)
                .Where(e => e.enabled);
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
        public abstract IEnumerable<ParameterEmote> CollectParameterEmotes();
        public abstract IEnumerable<AnimationMixin> CollectMixins();

        public abstract string LayerName { get; }
        public abstract string HandSignOverrideParameter { get;  }

        public bool HasLegacyData => legacyBaseMixins.Any() || legacyEmotes.Any() || legacyParameterEmotes.Any() || legacyMixins.Any();

        [Obsolete]
        public void RefreshParameters(ParametersWizard parametersWizard) => RefreshParameters(parametersWizard.parameterItems, parametersWizard.defaultParameterItems);

        [Obsolete]
        void RefreshParameters(IEnumerable<ParameterItem> parameterItems, IEnumerable<ParameterItem> defaultParameterItems)
        {
            var allParameterItems = parameterItems.Select(p => (p, false))
                .Concat(defaultParameterItems.Select(p => (p, true)))
                .Where(tuple => tuple.p.enabled)
                .ToList();
            var oldParameters = legacyParameterEmotes ?? new List<ParameterEmote>();
            legacyParameterEmotes = 
                Enumerable.Empty<ParameterEmote>()
                    .Concat(allParameterItems
                        .Select(tuple =>
                        {
                            var (parameterItem, isDefault) = tuple;
                            var parameter = oldParameters.FirstOrDefault(oldParameter => oldParameter.parameter == parameterItem.name) ?? new ParameterEmote
                            {
                                name = parameterItem.name,
                                parameter = parameterItem.name,
                                emoteKind = isDefault ? ParameterEmoteKind.Unused : ParameterEmoteKind.Transition,
                                enabled = !isDefault
                            };
                            parameter.valueKind = parameterItem.ValueKind;
                            parameter.enabled = parameter.enabled;
                            parameter.CollectStates(parameterItem);
                            return parameter;
                        }))
                    .Concat(oldParameters.Where(oldParameter =>
                        {
                            return allParameterItems.Select(tuple => tuple.p).All(parameterItem => oldParameter.parameter != parameterItem.name);
                        }).Select(oldParameter =>
                        {
                            oldParameter.enabled = false;
                            return oldParameter;
                        }))
                    .ToList();
        }
    }
}