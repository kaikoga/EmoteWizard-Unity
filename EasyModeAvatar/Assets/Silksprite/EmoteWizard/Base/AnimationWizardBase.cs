using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class AnimationWizardBase : EmoteWizardBase
    {
        [SerializeField] public bool advancedAnimations;
        [SerializeField] public bool handSignOverrideEnabled;

        [SerializeField] public List<AnimationMixin> baseMixins;
        [SerializeField] public List<Emote> emotes;
        [SerializeField] public List<ParameterEmote> parameterEmotes;
        [SerializeField] public List<AnimationMixin> mixins;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        public abstract string LayerName { get; }
        public abstract string HandSignOverrideParameter { get;  }

        public IEnumerable<ParameterEmote> ActiveParameters => parameterEmotes.Where(parameter => parameter.enabled && parameter.emoteKind != ParameterEmoteKind.Unused);

        public void RefreshParameters(ParametersWizard parametersWizard) => RefreshParameters(parametersWizard.parameterItems, parametersWizard.defaultParameterItems);

        void RefreshParameters(IEnumerable<ParameterItem> parameterItems, IEnumerable<ParameterItem> defaultParameterItems)
        {
            var allParameterItems = parameterItems.Select(p => (p, false))
                .Concat(defaultParameterItems.Select(p => (p, true)))
                .Where(tuple => tuple.p.enabled)
                .ToList();
            var oldParameters = parameterEmotes ?? new List<ParameterEmote>();
            parameterEmotes = 
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