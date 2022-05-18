using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    public abstract class ParameterEmoteSourceBase : EmoteWizardDataSourceBase
    {
        [SerializeField] public List<ParameterEmote> parameterEmotes;
        
        public abstract string LayerName { get; }

        public void GenerateParameters(ParametersWizard parametersWizard, AnimationWizardBase animationWizardBase) => GenerateParameters(parametersWizard.parameterItems, parametersWizard.defaultParameterItems, animationWizardBase);


        void GenerateParameters(IEnumerable<ParameterItem> parameterItems, IEnumerable<ParameterItem> defaultParameterItems, AnimationWizardBase animationWizardBase)
        {
            var allParameterItems = parameterItems.Select(p => (p, false))
                .Concat(defaultParameterItems.Select(p => (p, true)))
                .Where(tuple => tuple.p.enabled)
                .ToList();
            var oldParameters = parameterEmotes ?? new List<ParameterEmote>();
            parameterEmotes = new List<ParameterEmote>();
            var existingParameters = animationWizardBase.CollectParameterEmotes(true, true).Select(p => p.parameter).ToList();
            parameterEmotes = 
                Enumerable.Empty<ParameterEmote>()
                    .Concat(allParameterItems
                        .Where(tuple => !existingParameters.Contains(tuple.p.name))
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