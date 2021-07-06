using System.Collections.Generic;
using System.Linq;
using EmoteWizard.DataObjects;
using UnityEngine;

namespace EmoteWizard.Base
{
    public abstract class AnimationWizardBase : EmoteWizardBase
    {
        [SerializeField] public bool advancedAnimations;

        [SerializeField] public List<Emote> emotes;
        [SerializeField] public List<ParameterEmote> parameters;
        [SerializeField] public List<AnimationMixin> mixins;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        public abstract string LayerName { get; }
        public IEnumerable<ParameterEmote> ActiveParameters => parameters.Where(parameter => parameter.enabled);

        public void RefreshParameters(List<ParameterItem> parameterItems)
        {
            parameterItems = parameterItems.ToList();
            var oldParameters = parameters;
            parameters = 
                Enumerable.Empty<ParameterEmote>()
                    .Concat(parameterItems
                        .Select(parameterItem =>
                        {
                            var parameter = oldParameters.FirstOrDefault(oldParameter => oldParameter.parameter == parameterItem.name) ?? new ParameterEmote
                            {
                                name = parameterItem.name,
                                parameter = parameterItem.name
                            };
                            parameter.valueKind = parameterItem.valueKind;
                            parameter.enabled = true;
                            parameter.CollectStates(parameterItem);
                            return parameter;
                        }))
                    .Concat(oldParameters.Where(oldParameter =>
                        {
                            return parameterItems.All(parameterItem => oldParameter.parameter != parameterItem.name);
                        }).Select(oldParameter =>
                        {
                            oldParameter.enabled = false;
                            return oldParameter;
                        }))
                    .ToList();
        }
    }
}