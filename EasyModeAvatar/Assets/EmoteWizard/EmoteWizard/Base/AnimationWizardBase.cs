using System.Collections.Generic;
using System.Linq;
using EmoteWizard.DataObjects;
using UnityEngine;

namespace EmoteWizard.Base
{
    public abstract class AnimationWizardBase : EmoteWizardBase
    {
        [SerializeField] public List<Emote> emotes;
        [SerializeField] public List<ParameterEmote> parameters;
        [SerializeField] public List<AnimationMixin> mixins;

        [SerializeField] public RuntimeAnimatorController outputAsset;

        public void RefreshParameters(List<ParameterItem> parameterItems)
        {
            parameterItems = parameterItems.ToList();
            var oldParameters = parameters;
            parameters = 
                Enumerable.Empty<ParameterEmote>()
                    .Concat(parameterItems
                        .Where(parameterItem => !parameterItem.defaultParameter)
                        .Select(parameterItem =>
                        {
                            var parameter = oldParameters.FirstOrDefault(oldParameter => oldParameter.parameter == parameterItem.name) ?? new ParameterEmote
                            {
                                name = parameterItem.name,
                                parameter = parameterItem.name,
                            };
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