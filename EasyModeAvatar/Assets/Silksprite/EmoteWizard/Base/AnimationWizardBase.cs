using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Silksprite.EmoteWizard.Base
{
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

        public IEnumerable<AnimationMixin> CollectBaseMixins() => legacyBaseMixins.Where(m => m.enabled);
        public IEnumerable<Emote> CollectEmotes() => legacyEmotes;
        public IEnumerable<ParameterEmote> CollectParameterEmotes() => legacyParameterEmotes.Where(e => e.enabled);
        public IEnumerable<AnimationMixin> CollectMixins() => legacyMixins.Where(m => m.enabled);

        public abstract string LayerName { get; }
        public abstract string HandSignOverrideParameter { get;  }

        public void RefreshParameters(ParametersWizard parametersWizard) => RefreshParameters(parametersWizard.parameterItems, parametersWizard.defaultParameterItems);

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