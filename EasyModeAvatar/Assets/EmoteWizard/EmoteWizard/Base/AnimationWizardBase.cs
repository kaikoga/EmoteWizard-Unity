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

        public void RefreshParameters(IEnumerable<ParameterItem> parameterItems)
        {
            var oldParameters = parameters;
            parameters = parameterItems
                .Where(parameterItem => !parameterItem.defaultParameter)
                .Select(parameterItem =>
                {
                    var parameter = oldParameters.FirstOrDefault(oldParameter => oldParameter.parameter == parameterItem.name) ?? new ParameterEmote
                    {
                        name = parameterItem.name,
                        parameter = parameterItem.name,
                    };
                    parameter.CollectStates(parameterItem);
                    return parameter;
                })
                .ToList();
        }
    }
}